// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Internal;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FluentInjections.Application
{
    public class FluentApplicationBuilder<TBuilder> : IApplicationBuilder<TBuilder>
            where TBuilder : IApplicationBuilder<TBuilder>
    {
        public IServiceCollection Services { get; } = new ServiceCollection();
        public TBuilder Builder => (TBuilder)(IApplicationBuilder<TBuilder>)this;
        protected IHostBuilder _hostBuilder;
        protected Action<IApplicationBuilder> _configureApp;
        protected bool _disposed = false;

        public FluentApplicationBuilder(Action<IApplicationBuilder>? configureApp = null)
        {
            _configureApp = configureApp ?? ((_) => { });
            _hostBuilder = Host.CreateDefaultBuilder();
        }

        public Task<IApplication<TBuilder>> BuildAsync(CancellationToken cancellationToken = default)
        {
            _hostBuilder.ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureServices(services =>
                {
                    foreach (var serviceDescriptor in Services)
                    {
                        services.Add(serviceDescriptor);
                    }
                });

                webBuilder.Configure(app =>
                {
                    _configureApp(app);
                });

            });

            var host = _hostBuilder.Build();
            return Task.FromResult<IApplication<TBuilder>>(new FluentApplication<TBuilder>(Builder, host));
        }

        public TInnerBuilder GetInnerBuilder<TInnerBuilder>()
        {
            return (TInnerBuilder)(object)_hostBuilder;
        }

        public TBuilder AddConfigurationSource(IConfigurationSource configurationSource)
        {
            _hostBuilder.ConfigureAppConfiguration(config =>
            {
                config.Add(configurationSource);
            });

            return Builder;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
            }
        }
    }
}
