// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions;
using FluentInjections.Attributes;
using FluentInjections.Internal.Discovery.Configuration;
using FluentInjections.Metadata;
using FluentInjections.Validation;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace FluentInjections.Internal.Discovery
{
    internal class ModuleDiscoveryService : IModuleDiscoveryService
    {
        private readonly ILogger<ModuleDiscoveryService> _logger;
        private readonly ModuleDiscoveryOptions _options;

        public ModuleDiscoveryService(ILogger<ModuleDiscoveryService> logger, IOptions<ModuleDiscoveryOptions> options)
        {
            Guard.NotNull(logger, nameof(logger));
            Guard.NotNull(options, nameof(options));
            _logger = logger;
            _options = options.Value;
        }

        public async IAsyncEnumerable<ModuleMetadata> DiscoverModulesAsync(
            IEnumerable<Assembly> assemblies = null!,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            assemblies ??= AppDomain.CurrentDomain.GetAssemblies();
            var allTypes = new ConcurrentBag<Type>();

            await Parallel.ForEachAsync(assemblies, new ParallelOptions { CancellationToken = cancellationToken }, (assembly, ct) =>
            {
                try
                {
                    foreach (var type in assembly.GetTypes())
                    {
                        allTypes.Add(type);
                    }
                }
                catch (ReflectionTypeLoadException ex)
                {
                    _logger.LogError(ex, $"Error loading types from assembly: {assembly.FullName}");
                    foreach (var loaderException in ex.LoaderExceptions)
                    {
                        _logger.LogError(loaderException, "Loader Exception");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Unexpected error loading types from assembly: {assembly.FullName}");
                }

                return ValueTask.CompletedTask;
            });

            var moduleMetadata = new ConcurrentBag<ModuleMetadata>();

            await Parallel.ForEachAsync(allTypes, new ParallelOptions { CancellationToken = cancellationToken }, (type, ct) =>
            {
                if (IsModuleType(type, out var configuratorType, out var componentType, out int priority, out IEnumerable<Type> dependencies))
                {
                    moduleMetadata.Add(new ModuleMetadata(type, configuratorType, componentType, priority, dependencies));
                }
                return ValueTask.CompletedTask;
            });

            foreach (var metadata in moduleMetadata.OrderBy(m => m.Priority))
            {
                yield return metadata;
            }
        }

        private bool IsModuleType(Type type, out Type configuratorType, out Type componentType, out int priority, out IEnumerable<Type> dependencies)
        {
            configuratorType = null!;
            componentType = null!;
            priority = 0;
            dependencies = null!;

            var interfaces = type.GetInterfaces();

            foreach (var iface in interfaces)
            {
                if (iface.IsGenericType && iface.GetGenericTypeDefinition() == typeof(IConfigurator<>))
                {
                    var genericArguments = iface.GetGenericArguments();

                    if (genericArguments.Length == 1)
                    {
                        configuratorType = iface;
                        componentType = genericArguments[0];

                        var priorityAttribute = type.GetCustomAttribute<ModulePriorityAttribute>();

                        if (priorityAttribute is not null)
                        {
                            priority = priorityAttribute.Priority;
                        }

                        var dependencyAttributes = type.GetCustomAttributes<ModuleDependencyAttribute>();

                        if (dependencyAttributes.Any())
                        {
                            dependencies = dependencyAttributes.Select(attr => attr.DependencyType).ToList();
                        }

                        return true;
                    }
                }
            }

            return false;
        }
    }
}
