// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions;
using FluentInjections.Abstractions.Adapters;
using FluentInjections.DependencyInjection;
using FluentInjections.Validation;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using System.Reflection;
using System.Threading;

namespace FluentInjections
{
    /// <summary>
    /// Represents a fluent orchestrator for building applications.
    /// </summary>
    /// <typeparam name="TApplication">The type of the application.</typeparam>
    public class FluentOrchestrator<TApplication> : IFluentOrchestrator<TApplication>
    {
        private Type? _adapterType;
        private Func<IServiceProvider, object>? _adapterFactory;
        private readonly IServiceCollection _services = new ServiceCollection();
        private readonly ILoggerFactory _loggerFactory;
        private readonly IConfiguration _configuration;
        private readonly Dictionary<string, Type> _adapterRegistry = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentOrchestrator{TApplication}"/> class.
        /// </summary>
        /// <param name="arguments">The arguments to pass to the application.</param>
        /// <param name="configuration">The configuration to use for the application.</param>
        /// <param name="services">The services to use for the application.</param>
        /// <param name="loggerFactory">The logger factory to use for the application.</param>
        public FluentOrchestrator(IConfiguration? configuration = null, IServiceCollection? services = null, ILoggerFactory? loggerFactory = null)
        {
            _configuration = configuration ?? new ConfigurationBuilder().Build();
            _services = services ?? new ServiceCollection();
            _loggerFactory = loggerFactory ?? LoggerFactory.Create(builder => builder.AddConsole());
        }

        /// <summary>
        /// Adds a service to the application.
        /// </summary>
        /// <typeparam name="TAdapter">The type of the adapter to use.</typeparam>
        /// <param name="frameworkIdentifier">The identifier of the framework to use the adapter for.</param>
        /// <returns>A <see cref="FluentOrchestrator{TApplication}"/> instance.</returns>
        public IFluentOrchestrator<TApplication> UseAdapter<TAdapter>()
            where TAdapter : IApplicationAdapter<TApplication>
        {
            try
            {
                _adapterType = typeof(TAdapter);
                _adapterFactory = sp => ActivatorUtilities.CreateInstance<TAdapter>(sp);
            }
            catch (Exception ex)
            {
                var logger = _loggerFactory.CreateLogger(this.GetType().Name);
                logger.LogError(ex, "Failed to create {Adapter}", typeof(TAdapter).Name);
                throw;
            }

            return this;
        }

        public async Task<IApplicationAdapter<TApplication>> BuildAsync(CancellationToken? cancellationToken = null)
        {
            CancellationToken ct = cancellationToken ?? CancellationToken.None;
            var services = new ServiceCollection();

            if (_adapterType is not null && _adapterFactory is not null)
            {
                services.AddSingleton(_adapterType, _adapterFactory);
            }
            else
            {
                foreach (var adapterType in _adapterRegistry.Values)
                {
                    _services.AddSingleton(adapterType);
                }
            }

            _services.AddSingleton(_configuration);
            _services.AddSingleton(_loggerFactory);

            _services.AddSingleton<IComponentResolverProvider, ComponentResolverProvider>(sp =>
            {
                IServiceProvider provider = sp.GetRequiredService<IServiceProvider>();
                ILoggerFactory loggerFactory = provider.GetRequiredService<ILoggerFactory>();
                return new ComponentResolverProvider(provider, loggerFactory);
            });

            var provider = _services.BuildServiceProvider();
            var resolverProvider = provider.GetRequiredService<IComponentResolverProvider>();

            var initializer = new FluentOrchestratorInitializer<IComponent, TApplication>(_configuration, _loggerFactory, resolverProvider);
            await initializer.InitializeAsync(ct).ConfigureAwait(false);

            var adapter = provider.GetRequiredService<IApplicationAdapter<TApplication>>();
            return adapter;
        }

        ///// <summary>
        ///// Builds the application.
        ///// </summary>
        ///// <param name="cancellationToken">The cancellation token.</param>
        ///// <returns>A <see cref="Task{IApplicationAbstraction}"/> instance.</returns>
        //public async Task<IApplicationAbstraction> BuildAsync(CancellationToken cancellationToken)
        //{
        //    _services.AddSingleton(_configuration);
        //    _services.AddSingleton(_loggerFactory);
        //    _services.AddSingleton<IComponentResolverProvider, ComponentResolverProvider>();

        //    foreach (var adapterType in _adapterRegistry.Values)
        //    {
        //        _services.AddSingleton(adapterType);
        //    }

        //    var serviceProvider = _services.BuildServiceProvider();
        //    var resolverProvider = serviceProvider.GetRequiredService<IComponentResolverProvider>();

        //    var initializer = new FluentOrchestratorInitializer<IComponent, TApplication>(_configuration, _loggerFactory, resolverProvider);
        //    await initializer.InitializeAsync(cancellationToken);

        //    return new ApplicationAbstraction(serviceProvider);
        //}

        //private class ApplicationAbstraction : IApplicationAbstraction
        //{
        //    private readonly IServiceProvider _serviceProvider;

        //    public ApplicationAbstraction(IServiceProvider serviceProvider)
        //    {
        //        _serviceProvider = serviceProvider;
        //    }

        //    public ValueTask DisposeAsync() => throw new NotImplementedException();

        //    public TService GetService<TService>()
        //    {
        //        return _serviceProvider.GetService<TService>()!;
        //    }
        //}
    }
}
