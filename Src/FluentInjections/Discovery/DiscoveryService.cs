// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FluentInjections.DependencyResolution;

namespace FluentInjections.Discovery
{
    internal class DiscoveryService<TItem> : IDiscoveryService<TItem>
    {
        private readonly DiscoveryConfiguration<TItem> _configuration;
        private IServiceProvider? _serviceProvider;
        private readonly IItemDiscoverer _itemDiscoverer;
        private readonly IDependencyResolver _dependencyResolver;
        private readonly ILogger<DiscoveryService<TItem>> _logger;

        public DiscoveryService(
                IDiscoveryConfiguration<TItem> configuration,
                IItemDiscoverer itemDiscoverer,
                IDependencyResolver dependencyResolver,
                ILogger<DiscoveryService<TItem>> logger)
        {
            _configuration = configuration as DiscoveryConfiguration<TItem> ?? throw new ArgumentNullException(nameof(configuration));
            _itemDiscoverer = itemDiscoverer ?? throw new ArgumentNullException(nameof(itemDiscoverer));
            _dependencyResolver = dependencyResolver ?? throw new ArgumentNullException(nameof(dependencyResolver));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IServiceCollection Services => _configuration.Services;

        public async Task<IServiceProvider> DiscoverAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var discoveredItems = await _itemDiscoverer.DiscoverItemsAsync<TItem>(
                    _configuration.Assembly,
                    _configuration.Filter ?? (_ => true),
                    cancellationToken);

                var executionGraph = await _dependencyResolver.BuildExecutionGraphAsync<TItem>(
                    discoveredItems,
                    cancellationToken);

                foreach (var item in discoveredItems)
                {
                    _configuration.Services.AddTransient(item.ItemType);
                }

                _serviceProvider = _configuration.Services.BuildServiceProvider();

                return _serviceProvider;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initializing service provider.");
                throw;
            }
        }

        public object? GetService(Type serviceType)
        {
            if (_serviceProvider is null)
            {
                _serviceProvider = DiscoverAsync().Result;
            }

            return _serviceProvider.GetService(serviceType);
        }

        public async ValueTask<object?> GetServiceAsync(Type serviceType, CancellationToken cancellationToken = default)
        {
            if (_serviceProvider is null)
            {
                _serviceProvider = await DiscoverAsync(cancellationToken);
            }

            return _serviceProvider.GetService(serviceType);
        }

        public async ValueTask DisposeAsync()
        {
            if (_serviceProvider is IAsyncDisposable asyncDisposable)
            {
                await asyncDisposable.DisposeAsync();
            }
            else if (_serviceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}
