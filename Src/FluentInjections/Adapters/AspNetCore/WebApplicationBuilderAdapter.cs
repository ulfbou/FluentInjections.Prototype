// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions;
using FluentInjections.Configuration;
using FluentInjections.Logging;

using Microsoft.AspNetCore.Builder;

using IConfigurationProvider = FluentInjections.Configuration.IConfigurationProvider;
using ILoggerFactoryProvider = FluentInjections.Logging.ILoggerFactoryProvider;

namespace FluentInjections.Adapters.AspNetCore
{
    public class WebApplicationBuilderAdapter : IConcreteBuilderAdapter<WebApplicationBuilder, WebApplication>
    {
        private readonly WebApplicationBuilder _innerBuilder;
        private readonly IConfigurationProvider _configurationProvider;
        private readonly ILoggerFactoryProvider _loggerFactoryProvider;

        public WebApplicationBuilderAdapter(WebApplicationBuilder builder, IConfigurationProvider configurationProvider = null!, ILoggerFactoryProvider loggerFactoryProvider = null!)
        {
            _innerBuilder = builder ?? throw new ArgumentNullException(nameof(builder));
            _configurationProvider = configurationProvider ?? NullConfigurationProvider.Instance;
            _loggerFactoryProvider = loggerFactoryProvider ?? NullLoggerFactoryProvider.Instance;
        }

        public WebApplicationBuilder ConcreteBuilder => _innerBuilder;
        public IConfigurationProvider ConfigurationProvider => _configurationProvider;
        public ILoggerFactoryProvider LoggerFactoryProvider => _loggerFactoryProvider;

        public async Task<WebApplication> BuildAsync() // Implementing IBuilderAdapter<WebApplication>.BuildAsync
        {
            return await Task.FromResult(_innerBuilder.Build()); // Example, might need to be sync in WebAppBuilder
        }
    }
}
