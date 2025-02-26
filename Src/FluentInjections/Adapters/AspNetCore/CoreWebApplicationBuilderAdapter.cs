// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions;
using FluentInjections.Configuration;
using FluentInjections.Logging;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using IConfigurationProvider = FluentInjections.Configuration.IConfigurationProvider;
using ILoggerFactoryProvider = FluentInjections.Logging.ILoggerFactoryProvider;

namespace FluentInjections.Adapters.AspNetCore
{
    public class CoreWebApplicationBuilderAdapter :
        IConcreteBuilderAdapter<WebApplicationBuilder, WebApplication>,
        IAppBuilderCore<CoreWebApplicationAdapter> // Builds CoreWebApplicationAdapter
    {
        private readonly WebApplicationBuilder _innerBuilder;

        public CoreWebApplicationBuilderAdapter(WebApplicationBuilder builder, IConfigurationProvider configurationProvider = null!, ILoggerFactoryProvider loggerFactoryProvider = null!)
        {
            _innerBuilder = builder ?? throw new ArgumentNullException(nameof(builder));
            ConfigurationProvider = configurationProvider ?? NullConfigurationProvider.Instance;
            LoggerFactoryProvider = loggerFactoryProvider ?? NullLoggerFactoryProvider.Instance;
        }

        public WebApplicationBuilder ConcreteBuilder => _innerBuilder;
        public IConfiguration Configuration => _innerBuilder.Configuration;
        public ILoggingBuilder Logging => _innerBuilder.Logging;

        public IConfigurationProvider ConfigurationProvider { get; }

        public ILoggerFactoryProvider LoggerFactoryProvider { get; }

        public Task<CoreWebApplicationAdapter> BuildAsync() // Builds CoreWebApplicationAdapter now
        {
            var webApplication = _innerBuilder.Build();
            return Task.FromResult(new CoreWebApplicationAdapter(webApplication, this)); // Pass builder adapter for relation
        }

        Task<WebApplication> IBuilderAdapter<WebApplication>.BuildAsync() => throw new NotImplementedException();
    }
}
