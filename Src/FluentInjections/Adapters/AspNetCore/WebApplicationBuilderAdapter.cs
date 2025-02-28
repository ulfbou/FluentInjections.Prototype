// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions.Adapters;
using FluentInjections.Configuration;
using FluentInjections.Logging;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

using IConfigurationProvider = FluentInjections.Configuration.IConfigurationProvider;
using ILoggerFactoryProvider = FluentInjections.Logging.ILoggerFactoryProvider;

namespace FluentInjections.Adapters.AspNetCore
{
    public class WebApplicationBuilderAdapter : IConcreteBuilderAdapter<IHostApplicationBuilder, WebApplication>
    {
        private readonly WebApplicationBuilder _innerBuilder;
        private readonly IConfigurationProvider _configurationProvider;
        private readonly ILoggerFactoryProvider _loggerFactoryProvider;

        public WebApplicationBuilderAdapter(IHostApplicationBuilder builder, IConfigurationProvider configurationProvider = null!, ILoggerFactoryProvider loggerFactoryProvider = null!)
        {
            _innerBuilder = builder as WebApplicationBuilder ?? throw new ArgumentException("Builder must be a WebApplicationBuilder", nameof(builder));
            _configurationProvider = configurationProvider ?? NullConfigurationProvider.Instance;
            _loggerFactoryProvider = loggerFactoryProvider ?? NullLoggerFactoryProvider.Instance;
        }

        public IHostApplicationBuilder ConcreteBuilder => _innerBuilder;
        public WebApplicationBuilder Builder => _innerBuilder;
        public IConfigurationProvider ConfigurationProvider => _configurationProvider;
        public ILoggerFactoryProvider LoggerFactoryProvider => _loggerFactoryProvider;

        public Task<WebApplication> BuildAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return Task.FromCanceled<WebApplication>(cancellationToken);
            }

            return Task.FromResult(_innerBuilder.Build());
        }
    }
}
