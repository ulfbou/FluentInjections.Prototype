// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Application;
using FluentInjections.Internal;
using FluentInjections.Validation;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using System.Reflection;

namespace FluentInjections.Internal
{
    internal sealed class BuilderDiscoverer<TBuilder>
        where TBuilder : IApplicationBuilder<TBuilder>
    {
        private readonly ILogger _logger;
        private readonly BuilderRegistry _registry;
        private readonly ILoggerFactory _loggerFactory;

        public BuilderDiscoverer(ILoggerFactory loggerFactory, BuilderRegistry builderRegistry)
        {
            Guard.NotNull(loggerFactory, nameof(loggerFactory));
            Guard.NotNull(builderRegistry, nameof(builderRegistry));
            _loggerFactory = loggerFactory;
            _logger = loggerFactory.CreateLogger(nameof(BuilderDiscoverer<TBuilder>));
            _registry = builderRegistry;
        }

        // TODO: Terminate the discovery process after TBuilder is found, if possible. 
        public void DiscoverAndRegisterBuilders()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var builders = assemblies
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsClass && !t.IsAbstract && t.GetCustomAttribute<BuilderAttribute>() != null)
                .Select(t => t.GetCustomAttribute<BuilderAttribute>()!)
                .ToList();

            // Filter invalid builder attributes and log warnings
            var validBuilders = new HashSet<BuilderAttribute>(builders.Where(b => b.IsValid));
            var invalidBuilders = new HashSet<BuilderAttribute>(builders.Except(validBuilders));

            foreach (var attribute in invalidBuilders.ToList())
            {
                _logger.LogWarning("Invalid builder attribute {Attribute}: {Errors}", attribute, string.Join(", ", attribute.Errors));
            }

            ParallelOptions options = new()
            {
                MaxDegreeOfParallelism = Environment.ProcessorCount,
                CancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(10)).Token
            };

            Parallel.ForEach(validBuilders.ToList(), options, attribute =>
            {
                try
                {
                    RegisterBuilder(attribute);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error registering builder type {BuilderType}", attribute.BuilderType);
                }
            });
        }

        private void RegisterBuilder(BuilderAttribute attribute)
        {
            var factoryType = typeof(IBuilderFactory<>).MakeGenericType(attribute.BuilderType);
            var factoryInstance = Activator.CreateInstance(attribute.BuilderFactoryType);

            if (!factoryType.IsInstanceOfType(factoryInstance))
            {
                _logger.LogWarning("Builder factory type {FactoryType} does not implement IBuilderFactory<{BuilderType}>",
                                   attribute.BuilderFactoryType,
                                   attribute.BuilderType);
                return;
            }

            var factoryMethod = attribute.BuilderFactoryType.GetMethod("CreateBuilder")?
                .MakeGenericMethod(attribute.BuilderType);

            if (factoryMethod is null)
            {
                _logger.LogWarning("Factory method for builder type {BuilderType} not found", attribute.BuilderType);
                return;
            }

            var factory = factoryMethod.Invoke(factoryInstance, new object[] { _loggerFactory }) as Delegate;

            if (factory is null)
            {
                _logger.LogWarning("Factory method for builder type {BuilderType} could not be created", attribute.BuilderType);
                return;
            }

            var registerMethod = typeof(BuilderRegistry).GetMethod(nameof(BuilderRegistry.RegisterBuilder))?
                .MakeGenericMethod(attribute.BuilderType);

            if (registerMethod is null)
            {
                _logger.LogWarning("Register method for builder type {BuilderType} not found", attribute.BuilderType);
                return;
            }

            registerMethod.Invoke(_registry, new object[] { factory });
        }
    }
}
