// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions.Adapters;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FluentInjections.Adapters.AspNetCore
{
    //public class CoreWebApplicationBuilderAdapter :
    //    IConcreteBuilderAdapter,
    //    IAppBuilderCore
    //{
    //    private readonly WebApplicationBuilder _innerBuilder;

    //    public CoreWebApplicationBuilderAdapter(IHostApplicationBuilder builder, IConfigurationProvider configurationProvider = null!)
    //    {
    //        _innerBuilder = builder as WebApplicationBuilder ?? throw new ArgumentException("Builder must be a WebApplicationBuilder", nameof(builder));
    //        //ConfigurationProvider = configurationProvider ?? NullConfigurationProvider.Instance;
    //        //LoggerFactoryProvider = loggerFactoryProvider ?? NullLoggerFactoryProvider.Instance;
    //        ConfigurationProvider = configurationProvider ?? throw new ArgumentNullException(nameof(configurationProvider));
    //    }

    //    public IHostApplicationBuilder ConcreteBuilder => _innerBuilder;
    //    public WebApplicationBuilder Builder => _innerBuilder;
    //    public IConfiguration Configuration => _innerBuilder.Configuration;
    //    public ILoggingBuilder Logging => _innerBuilder.Logging;

    //    public IConfigurationProvider ConfigurationProvider { get; }

    //    //public ILoggerFactoryProvider LoggerFactoryProvider { get; }

    //    public object BuildApplication() => throw new NotImplementedException();
    //}
}
