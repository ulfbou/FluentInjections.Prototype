// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Metadata;
using Microsoft.Extensions.DependencyInjection;
using FluentInjections.Abstractions;
using FluentInjections.Modules;
using FluentInjections.Validation;
using System.Collections.Concurrent;

namespace FluentInjections.Orchestration
{
    public class ModuleOrchestrator : IModuleOrchestrator
    {
        private IServiceProvider _externalServiceProvider;

        public ModuleOrchestrator(IServiceProvider serviceProvider)
        {
            Guard.NotNull(serviceProvider, nameof(serviceProvider));
            _externalServiceProvider = serviceProvider;
        }

        public async Task ExecuteModulesAsync<TComponent>(IAsyncEnumerable<ModuleMetadata> moduleMetadata, CancellationToken? cancellationToken = null)
            where TComponent : IComponent
        {
            Guard.NotNull(moduleMetadata, nameof(moduleMetadata));

            var services = new ServiceCollection();
            var ct = cancellationToken ?? CancellationToken.None;

            // Filter modules for the specified component type
            var filteredMetadata = new List<ModuleMetadata>();
            await foreach (var metadata in moduleMetadata)
            {
                if (ct.IsCancellationRequested)
                {
                    return;
                }
                if (metadata.ComponentType == typeof(TComponent))
                {
                    filteredMetadata.Add(metadata);
                }
            }

            filteredMetadata = filteredMetadata.OrderBy(m => m.Priority).ToList();

            var dependencies = filteredMetadata
                .SelectMany(m => m.Dependencies ?? Enumerable.Empty<Type>())
                .Distinct()
                .ToList();

            var configurators = filteredMetadata.Select(m => m.ConfiguratorType);

            if (configurators.Count() > 1)
            {
                throw new InvalidOperationException("Multiple configurators found for the specified component type.");
            }

            foreach (var dependency in dependencies)
            {
                if (ct.IsCancellationRequested)
                {
                    return;
                }

                services.AddSingleton(dependency);
            }

            services.AddSingleton(configurators.Single());

            foreach (var metadata in filteredMetadata)
            {
                if (ct.IsCancellationRequested)
                {
                    return;
                }

                services.AddSingleton(metadata.ModuleType);
            }

            IServiceProvider moduleServiceProvider = services.BuildServiceProvider();

            if (_externalServiceProvider != null)
            {
                moduleServiceProvider = new CompositeServiceProvider(moduleServiceProvider, _externalServiceProvider);
            }

            var configurator = moduleServiceProvider.GetRequiredService<IConfigurator<TComponent>>();
            var modules = moduleServiceProvider.GetServices<IConfigurableModule<IConfigurator<TComponent>>>();

            foreach (var module in modules)
            {
                await module.ConfigureAsync(configurator, ct).ConfigureAwait(false);
            }
        }

        internal class CompositeServiceProvider : IServiceProvider
        {
            private readonly IServiceProvider _modulesServiceProvider;
            private readonly IServiceProvider _externalServiceProvider;

            public CompositeServiceProvider(IServiceProvider modulesServiceProvider, IServiceProvider externalServiceProvider)
            {
                Guard.NotNull(modulesServiceProvider, nameof(modulesServiceProvider));
                Guard.NotNull(externalServiceProvider, nameof(externalServiceProvider));
                _modulesServiceProvider = modulesServiceProvider;
                _externalServiceProvider = externalServiceProvider;
            }

            public object? GetService(Type serviceType)
            {
                var service = _modulesServiceProvider.GetService(serviceType);
                return service ?? _externalServiceProvider.GetService(serviceType);
            }
        }
    }
}
