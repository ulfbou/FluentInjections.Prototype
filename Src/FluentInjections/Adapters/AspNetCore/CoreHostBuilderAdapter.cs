// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions;
using FluentInjections.Abstractions.Adapters;
using FluentInjections.Configuration;
using FluentInjections.Logging;

using Microsoft.Extensions.Hosting;

using System.Threading;

namespace FluentInjections.Adapters.AspNetCore
{
    public class CoreHostBuilderAdapter :
        IConcreteBuilderAdapter<IHostBuilder, IHost>,
        IAppBuilderCore<CoreHostApplicationAdapter>
    {
        private readonly HostBuilder _innerBuilder;
        private readonly IConfigurationProvider _configurationProvider;
        private readonly ILoggerFactoryProvider _loggerFactoryProvider;

        public CoreHostBuilderAdapter(IHostBuilder builder, IConfigurationProvider configurationProvider, ILoggerFactoryProvider loggerFactoryProvider)
        {
            _innerBuilder = builder as HostBuilder ?? throw new ArgumentException("Builder must be a HostBuilder", nameof(builder));
            _configurationProvider = configurationProvider ?? throw new ArgumentNullException(nameof(configurationProvider));
            _loggerFactoryProvider = loggerFactoryProvider ?? throw new ArgumentNullException(nameof(loggerFactoryProvider));
        }

        public IHostBuilder ConcreteBuilder => _innerBuilder;
        public IConfigurationProvider ConfigurationProvider => _configurationProvider;
        public ILoggerFactoryProvider LoggerFactoryProvider => _loggerFactoryProvider;

        public Task<CoreHostApplicationAdapter> BuildAsync(CancellationToken? cancellationToken = null)
        {
            if (cancellationToken.HasValue && cancellationToken.Value.IsCancellationRequested)
            {
                return Task.FromCanceled<CoreHostApplicationAdapter>(cancellationToken.Value);
            }

            var host = _innerBuilder.Build();
            return Task.FromResult(new CoreHostApplicationAdapter(host, this));
        }

        public static explicit operator CoreHostBuilderAdapter(CoreWebApplicationBuilderAdapter adapter)
        {
            return new CoreHostBuilderAdapter(adapter.Builder.Host, adapter.ConfigurationProvider, adapter.LoggerFactoryProvider);
        }

        // No IWebAppMiddlewareBuilderExtension implementation - HostBuilder doesn't have web middleware
        // Could potentially have other extensions later for HostBuilder-specific features (e.g., service config extensions)
    }
}
