// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions;
using FluentInjections.Abstractions.Adapters;
using FluentInjections.Adapters.AspNetCore;
using FluentInjections.Attributes;
using FluentInjections.Core.Abstractions;
using FluentInjections.Core.Caching;
using FluentInjections.Core.Configuration;
using FluentInjections.Core.Discovery;
using FluentInjections.Core.Discovery.Metadata;
using FluentInjections.Core.Filters;
using FluentInjections.Modules;
using FluentInjections.Validation;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using System.Reflection;

namespace FluentInjections
{
    /// <summary>
    /// Represents a fluent orchestrator for building applications.
    /// </summary>
    /// <typeparam name="TApplication">The type of the application.</typeparam>
    public class FluentOrchestrator<TApplication> : IFluentOrchestrator<TApplication>
    {
        public static string ConfigurationSectionName => nameof(FluentInjections);

        private IConfiguration _configuration;
        private ILoggerFactory _loggerFactory;
        private TypeDiscoveryOptions _discoveryOptions;
        private readonly string[] _arguments;
        private readonly Dictionary<string, object> _adapterRegistry;
        private readonly IApplicationTypeRegistry _applicationTypeRegistry;
        private readonly IServiceCollection _services;

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentOrchestrator{TApplication}" /> class.
        /// </summary>
        /// <param name="arguments">The arguments to pass to the application.</param>
        /// <param name="configuration">The configuration to use for the application.</param>
        /// <param name="loggerFactory">The logger factory to use for the application.</param>
        public FluentOrchestrator(string[]? arguments = null, IConfiguration? configuration = null, ILoggerFactory? loggerFactory = null, IServiceCollection? services = null)
        {
            _arguments = arguments ?? Array.Empty<string>();
            _configuration = configuration ?? new ConfigurationBuilder().Build();
            _loggerFactory = loggerFactory ?? LoggerFactory.Create(builder => builder.AddConsole());
            _discoveryOptions = TypeDiscoveryOptions.DefaultOptions;
            _adapterRegistry = new Dictionary<string, object>();
            _applicationTypeRegistry = new ApplicationTypeRegistry();
            _services = services ?? new ServiceCollection();
        }

        /// <summary>
        /// Adds a service to the application.
        /// </summary>
        /// <typeparam name="TAdapter">The type of the adapter to use.</typeparam>
        /// <param name="frameworkIdentifier">The identifier of the framework to use the adapter for.</param>
        /// <param name="adapterInstance">The instance of the adapter to use. Defaults to <see langword="default" />.</param>
        /// <returns>A <see cref="FluentOrchestrator{TApplication}" /> instance.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the adapter type does not have an <see cref="AdapterMetadataAttribute" />.</exception>
        /// <remarks>
        /// <para>
        /// This method registers an adapter for a specific framework identifier. The adapter instance can be provided explicitly or 
        /// </para>
        /// </remarks>
        public IFluentOrchestrator<TApplication> UseAdapterInstance(string frameworkIdentifier, object adapterInstance)
        {
            Guard.NotNullOrWhiteSpace(frameworkIdentifier, nameof(frameworkIdentifier));
            Guard.NotNull(adapterInstance, nameof(adapterInstance));

            if (!(adapterInstance is IApplicationBuilderAdapter<TApplication>))
            {
                throw new InvalidOperationException("Adapter instance must implement IApplicationBuilderAdapter<TApplication>.");
            }

            _adapterRegistry[frameworkIdentifier] = adapterInstance;
            return this;
        }

        public IFluentOrchestrator<TApplication> UseAdapter<TAdapter>(string frameworkIdentifier)
                where TAdapter : IApplicationAdapter<TApplication>
        {
            Guard.NotNullOrWhiteSpace(frameworkIdentifier, nameof(frameworkIdentifier));

            var adapterType = typeof(TAdapter);
            var metadataAttributes = adapterType.GetCustomAttributes<AdapterMetadataAttribute>();
            var metadataAttribute = metadataAttributes.Count() switch
            {
                1 => metadataAttributes.FirstOrDefault(),
                0 => throw new InvalidOperationException($"Adapter type {adapterType.FullName} does not have an AdapterMetadataAttribute."),
                _ => metadataAttributes.FirstOrDefault(a => a.FrameworkIdentifier == frameworkIdentifier)
                        ?? throw new InvalidOperationException($"Adapter type {adapterType.FullName} does not have an AdapterMetadataAttribute for framework identifier {frameworkIdentifier}.")
            };

            _adapterRegistry[frameworkIdentifier] = adapterType;
            return this;
        }

        public IFluentOrchestrator<TApplication> WithDiscoveryOptions(TypeDiscoveryOptions options)
        {
            Guard.NotNull(options, nameof(options));
            _discoveryOptions = options;
            return this;
        }

        public async Task<TApplication> BuildAsync(CancellationToken cancellationToken = default)
        {
            _services.AddSingleton(_configuration);
            _services.AddSingleton(_loggerFactory);

            var fluentInjectionsSection = _configuration.GetSection(nameof(FluentInjections))?.Get<TypeDiscoveryOptions>() ?? TypeDiscoveryOptions.DefaultOptions;

            _services.AddSingleton<TypeDiscoveryCache>();
            _services.AddSingleton<AssemblyScanner>();
            _services.AddSingleton<ITypeDiscoveryStrategy>(sp => new AttributeDiscoveryStrategy(sp.GetRequiredService<ILoggerFactory>()));
            _services.AddSingleton<TypeDiscoveryPipeline>();
            _services.AddLogging();

            var serviceProvider = _services.BuildServiceProvider();

            try
            {
                var pipeline = serviceProvider.GetRequiredService<TypeDiscoveryPipeline>();
                var context = CreateDiscoveryContext();
                var discoveredTypes = new List<Type>();

                await foreach (var type in pipeline.DiscoverTypes(context, cancellationToken))
                {
                    discoveredTypes.Add(type);
                }

                await RegisterAdapters(_services, pipeline, context, cancellationToken);

                var combinedServiceProvider = _services.BuildServiceProvider();
                var builderAdapter = ResolveBuilderAdapter(combinedServiceProvider, _services);

                await ExecuteModules<ILifecycleComponent>(discoveredTypes, combinedServiceProvider, cancellationToken);
                await ExecuteModules<IServiceComponent>(discoveredTypes, combinedServiceProvider, cancellationToken);

                IApplicationAdapter<TApplication> applicationAdapter = await builderAdapter.BuildAsync(cancellationToken);

                await ExecuteModules<IMiddlewareComponent>(discoveredTypes, combinedServiceProvider, cancellationToken);
                return applicationAdapter.Application;
            }
            catch (Exception ex)
            {
                _loggerFactory.CreateLogger<FluentOrchestrator<TApplication>>().LogError(ex, "Error building application.");
                throw;
            }
        }

        private IApplicationBuilderAdapter<TApplication> ResolveBuilderAdapter(ServiceProvider combinedServiceProvider, IServiceCollection services)
        {
            var adapter = _adapterRegistry.Values.FirstOrDefault();

            if (adapter is null)
            {
                throw new InvalidOperationException("No Adapter registered");
            }

            IApplicationBuilderAdapter<TApplication> builderAdapter;

            if (adapter is Type adapterType)
            {
                builderAdapter = (IApplicationBuilderAdapter<TApplication>)combinedServiceProvider.GetRequiredService(adapterType);
            }
            else if (adapter is IApplicationBuilderAdapter<TApplication> adapterInstance)
            {
                builderAdapter = adapterInstance;
            }
            else
            {
                throw new InvalidOperationException("Invalid adapter type registered.");
            }

            builderAdapter.ConfigureServices(services);

            return builderAdapter;
        }

        private TypeDiscoveryContext CreateDiscoveryContext()
        {
            TypeDiscoveryOptions options = _configuration.GetSection("FluentInjections")?.Get<TypeDiscoveryOptions>() ?? TypeDiscoveryOptions.DefaultOptions;

            var filter = new PredicateAssemblyFilter(a => true);

            return new TypeDiscoveryContext
            {
                AssemblyFilter = filter,
                InterfaceType = (string.IsNullOrEmpty(options?.InterfaceTypeName) ? null : Type.GetType(options.InterfaceTypeName))!,
                AttributeType = (string.IsNullOrEmpty(options?.AttributeTypeName) ? null : Type.GetType(options.AttributeTypeName))!,
                Assemblies = AppDomain.CurrentDomain.GetAssemblies()
            };
        }

        private async Task RegisterAdapters(IServiceCollection services, TypeDiscoveryPipeline pipeline, TypeDiscoveryContext context, CancellationToken cancellationToken)
        {
            var instanceAdapters = _adapterRegistry.Values.OfType<IApplicationBuilderAdapter<TApplication>>().ToList();
            var typeAdapters = _adapterRegistry.Values.OfType<Type>().ToList();
            var otherAdapters = _adapterRegistry.Values.Except(instanceAdapters).Except(typeAdapters).ToList();

            // Register instance adapters first
            foreach (var adapter in instanceAdapters)
            {
                services.AddSingleton(adapter);
            }

            // Only register type adapters if no instance is already registered for the same type
            foreach (var adapterType in typeAdapters)
            {
                if (!services.Any(sd => sd.ServiceType == adapterType && sd.ImplementationInstance is IApplicationBuilderAdapter<TApplication>))
                {
                    services.AddSingleton(adapterType);
                }
            }

            foreach (var adapter in otherAdapters)
            {
                _loggerFactory.CreateLogger<FluentOrchestrator<TApplication>>().LogError($"Invalid adapter type {adapter.GetType().FullName}. Skipping adapter.");
            }


            context.AttributeType = typeof(AdapterMetadataAttribute);
            var adapterTypes = new List<Type>();

            await foreach (var type in pipeline.DiscoverTypes(context, cancellationToken))
            {
                adapterTypes.Add(type);
            }

            foreach (var adapterType in adapterTypes)
            {
                var metadataAttribute = adapterType.GetCustomAttribute<AdapterMetadataAttribute>();

                if (metadataAttribute != null && !_adapterRegistry.ContainsValue(adapterType))
                {
                    // Directly instantiate the adapter using its constructor
                    try
                    {
                        var constructor = adapterType.GetConstructors()
                                                        .OrderByDescending(c => c.GetParameters().Length)
                                                        .FirstOrDefault();

                        if (constructor == null)
                        {
                            throw new InvalidOperationException($"Adapter type {adapterType.FullName} does not have a public constructor.");
                        }

                        var parameters = constructor.GetParameters();
                        var parameterInstances = new object?[parameters.Length];

                        for (int i = 0; i < parameters.Length; i++)
                        {
                            var parameterType = parameters[i].ParameterType;
                            // Attempt to resolve constructor parameters from the service collection
                            // parameterInstances[i] = services.BuildServiceProvider().GetService(parameterType)!;
                            parameterInstances[i] = services.FirstOrDefault(sd => sd.ServiceType == parameterType)?.ImplementationInstance;

                            if (parameterInstances[i] == null)
                            {
                                _loggerFactory.CreateLogger<FluentOrchestrator<TApplication>>().LogError($"Could not resolve parameter {parameterType.FullName} for adapter {adapterType.FullName}.");
                                throw new InvalidOperationException($"Could not resolve parameter {parameterType.FullName} for adapter {adapterType.FullName}.");
                            }
                        }

                        var adapterInstance = Activator.CreateInstance(adapterType, parameterInstances);

                        if (adapterInstance == null)
                        {
                            throw new InvalidOperationException($"Could not instantiate adapter {adapterType.FullName}.");
                        }

                        services.AddSingleton(adapterType, adapterInstance);
                    }
                    catch (Exception ex)
                    {
                        // Handle instantiation exceptions (e.g., missing dependencies)
                        // Log the exception and potentially skip the registration
                        _loggerFactory.CreateLogger<FluentOrchestrator<TApplication>>().LogError(ex, $"Error registering adapter {adapterType.FullName}.");
                    }
                }
            }
        }

        private async Task RegisterAdapters2(IServiceCollection services, TypeDiscoveryPipeline pipeline, TypeDiscoveryContext context, CancellationToken cancellationToken)
        {
            var instanceAdapters = _adapterRegistry.Values.OfType<IApplicationBuilderAdapter<TApplication>>().ToList();
            var typeAdapters = _adapterRegistry.Values.OfType<Type>().ToList();
            var otherAdapters = _adapterRegistry.Values.Except(instanceAdapters).Except(typeAdapters).ToList();

            foreach (var adapter in instanceAdapters)
            {
                services.AddSingleton(adapter);
            }

            foreach (var adapter in typeAdapters)
            {
                services.AddSingleton(adapter);
            }

            foreach (var adapter in otherAdapters)
            {
                _loggerFactory.CreateLogger<FluentOrchestrator<TApplication>>().LogError($"Invalid adapter type {adapter.GetType().FullName}. Skipping adapter.");
            }

            context.AttributeType = typeof(AdapterMetadataAttribute);
            var adapterTypes = new List<Type>();

            await foreach (var type in pipeline.DiscoverTypes(context, cancellationToken))
            {
                adapterTypes.Add(type);
            }

            foreach (var adapterType in adapterTypes)
            {
                var metadataAttribute = adapterType.GetCustomAttribute<AdapterMetadataAttribute>();
                if (metadataAttribute != null && !_adapterRegistry.ContainsValue(adapterType))
                {
                    // Directly instantiate the adapter using its constructor
                    try
                    {
                        var constructor = adapterType.GetConstructors()
                                                        .OrderByDescending(c => c.GetParameters().Length)
                                                        .FirstOrDefault();

                        if (constructor == null)
                        {
                            throw new InvalidOperationException($"Adapter type {adapterType.FullName} does not have a public constructor.");
                        }

                        var parameters = constructor.GetParameters();
                        var parameterInstances = new object?[parameters.Length];

                        for (int i = 0; i < parameters.Length; i++)
                        {
                            var parameterType = parameters[i].ParameterType;
                            // Attempt to resolve constructor parameters from the service collection
                            // parameterInstances[i] = services.BuildServiceProvider().GetService(parameterType)!;
                            parameterInstances[i] = services.FirstOrDefault(sd => sd.ServiceType == parameterType)?.ImplementationInstance;

                            if (parameterInstances[i] == null)
                            {
                                _loggerFactory.CreateLogger<FluentOrchestrator<TApplication>>().LogError($"Could not resolve parameter {parameterType.FullName} for adapter {adapterType.FullName}.");
                                throw new InvalidOperationException($"Could not resolve parameter {parameterType.FullName} for adapter {adapterType.FullName}.");
                            }
                        }

                        var adapterInstance = Activator.CreateInstance(adapterType, parameterInstances);

                        if (adapterInstance == null)
                        {
                            throw new InvalidOperationException($"Could not instantiate adapter {adapterType.FullName}.");
                        }

                        services.AddSingleton(adapterType, adapterInstance);
                    }
                    catch (Exception ex)
                    {
                        // Handle instantiation exceptions (e.g., missing dependencies)
                        // Log the exception and potentially skip the registration
                        _loggerFactory.CreateLogger<FluentOrchestrator<TApplication>>().LogError(ex, $"Error registering adapter {adapterType.FullName}.");
                    }
                }
            }
        }

        private async Task ExecuteModules<TComponent>(IEnumerable<Type> moduleTypes, IServiceProvider serviceProvider, CancellationToken cancellationToken)
            where TComponent : IComponent
        {
            var filteredModules = moduleTypes
                .Where(type => type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IConfigurableModule<IConfigurator<TComponent>>)))
                .ToList();

            var sortedModules = TopologicalSort(filteredModules);

            foreach (var moduleType in sortedModules)
            {
                var configuratorType = moduleType.GetInterfaces()
                    .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IConfigurableModule<IConfigurator<TComponent>>))
                    .GetGenericArguments()[0];

                var configurator = (IConfigurator<TComponent>)serviceProvider.GetRequiredService(configuratorType);
                var module = (IConfigurableModule<IConfigurator<TComponent>>)serviceProvider.GetRequiredService(moduleType);

                await module.ConfigureAsync(configurator, cancellationToken);
            }
        }

        private List<Type> TopologicalSort(List<Type> modules)
        {
            var sorted = new List<Type>();
            var visited = new HashSet<Type>();
            var temp = new HashSet<Type>();

            foreach (var module in modules)
            {
                Visit(module, visited, temp, sorted);
            }

            return sorted;
        }

        private void Visit(Type module, HashSet<Type> visited, HashSet<Type> temp, List<Type> sorted)
        {
            if (temp.Contains(module))
            {
                throw new InvalidOperationException("Circular dependency detected.");
            }

            if (visited.Contains(module))
            {
                return;
            }

            temp.Add(module);

            var dependencyAttributes = module.GetCustomAttributes<ModuleDependencyAttribute>();

            foreach (var dependencyAttribute in dependencyAttributes)
            {
                Visit(dependencyAttribute.DependencyType, visited, temp, sorted);
            }

            temp.Remove(module);
            visited.Add(module);
            sorted.Add(module);
        }
    }
}
