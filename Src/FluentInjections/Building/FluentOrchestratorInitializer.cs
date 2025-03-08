// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions;
using FluentInjections.Abstractions.Adapters;
using FluentInjections.DependencyInjection;
using FluentInjections.Internal.Discovery;
using FluentInjections.Internal.Discovery.Configuration;
using FluentInjections.Metadata;
using FluentInjections.Orchestration;
using FluentInjections.Validation;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FluentInjections
{
    internal class FluentOrchestratorInitializer<TComponent, TApplication>
        where TComponent : IComponent
    {
        private readonly IConfiguration _configuration;
        private readonly ILoggerFactory _loggerFactory;
        private readonly IComponentResolverProvider _resolverProvider;
        private readonly string[] _arguments;
        private readonly Dictionary<string, Type> _adapterRegistry;

        public FluentOrchestratorInitializer(IConfiguration configuration, ILoggerFactory loggerFactory, IComponentResolverProvider resolverProvider, string[] arguments, Dictionary<string, Type> adapterRegistry)
        {
            Guard.NotNull(configuration, nameof(configuration));
            Guard.NotNull(loggerFactory, nameof(loggerFactory));
            Guard.NotNull(resolverProvider, nameof(resolverProvider));
            Guard.NotNull(arguments, nameof(arguments));
            Guard.NotNull(adapterRegistry, nameof(adapterRegistry));
            _configuration = configuration;
            _loggerFactory = loggerFactory;
            _resolverProvider = resolverProvider;
            _arguments = arguments;
            _adapterRegistry = adapterRegistry;
        }

        internal async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            var options = Options.Create(new ModuleDiscoveryOptions());
            var discovery = new ModuleDiscoveryService(_loggerFactory.CreateLogger<ModuleDiscoveryService>(), options);
            var metadata = discovery.DiscoverModulesAsync(cancellationToken: cancellationToken);

            var orchestrator = new ModuleOrchestrator(_resolverProvider.GetComponentResolver<TComponent>(), _loggerFactory);
            await orchestrator.ExecuteModulesAsync<TComponent>(metadata, cancellationToken).ConfigureAwait(false);

            await RegisterAdapters(cancellationToken);
        }

        private async Task RegisterAdapters(CancellationToken cancellationToken)
        {
            var services = new ServiceCollection();
            _resolverProvider.SetServiceProvider(serviceProvider);
            var options = Options.Create(new ModuleDiscoveryOptions());
            var discovery = new ModuleDiscoveryService(_loggerFactory.CreateLogger<ModuleDiscoveryService>(), options);
            var metadata = discovery.DiscoverModulesAsync(cancellationToken: cancellationToken);
            var sp = new ServiceCollection().BuildServiceProvider();
            var orchestrator = new ModuleOrchestrator(sp);
            await orchestrator.ExecuteModulesAsync<TComponent>(metadata, cancellationToken).ConfigureAwait(false);
            services.AddSingleton(_configuration);
            services.AddSingleton(_loggerFactory);
            services.AddSingleton(_resolverProvider);

            var adapterRegistrar = new AdapterRegistrar(_adapterRegistry, _resolverProvider.GetComponentResolver<TComponent>(), _loggerFactory, services);
            await adapterRegistrar.RegisterAdapters(cancellationToken);

            var serviceProvider = services.BuildServiceProvider();
        }
    }
}
