// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions;
using FluentInjections.Abstractions.Adapters;
using FluentInjections.Configuration;
using FluentInjections.Logging;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using IConfigurationProvider = FluentInjections.Configuration.IConfigurationProvider;
using ILoggerFactoryProvider = FluentInjections.Logging.ILoggerFactoryProvider;

namespace FluentInjections.Adapters.AspNetCore
{
    public class CoreWebApplicationBuilderAdapter :
        IConcreteBuilderAdapter<IHostApplicationBuilder, IApplicationBuilder>,
        IAppBuilderCore<CoreWebApplicationAdapter>
    {
        private readonly WebApplicationBuilder _innerBuilder;

        public CoreWebApplicationBuilderAdapter(IHostApplicationBuilder builder, IConfigurationProvider configurationProvider = null!, ILoggerFactoryProvider loggerFactoryProvider = null!)
        {
            _innerBuilder = builder as WebApplicationBuilder ?? throw new ArgumentException("Builder must be a WebApplicationBuilder", nameof(builder));
            ConfigurationProvider = configurationProvider ?? NullConfigurationProvider.Instance;
            LoggerFactoryProvider = loggerFactoryProvider ?? NullLoggerFactoryProvider.Instance;
        }

        public IHostApplicationBuilder ConcreteBuilder => _innerBuilder;
        public WebApplicationBuilder Builder => _innerBuilder;
        public IConfiguration Configuration => _innerBuilder.Configuration;
        public ILoggingBuilder Logging => _innerBuilder.Logging;

        public IConfigurationProvider ConfigurationProvider { get; }

        public ILoggerFactoryProvider LoggerFactoryProvider { get; }

        public Task<CoreWebApplicationAdapter> BuildAsync(CancellationToken? cancellationToken = null)
        {
            if (cancellationToken.HasValue && cancellationToken.Value.IsCancellationRequested)
            {
                return Task.FromCanceled<CoreWebApplicationAdapter>(cancellationToken.Value);
            }

            var webApplication = _innerBuilder.Build();
            return Task.FromResult(new CoreWebApplicationAdapter(webApplication, this));
        }
    }
}
