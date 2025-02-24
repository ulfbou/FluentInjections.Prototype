// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Application;
using FluentInjections.Configurators;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace FluentInjections.Middlewares
{
    public class MiddlewareConfigurator<TBuilder> : ConfiguratorBase<IMiddlewareComponent>, IMiddlewareConfigurator<TBuilder>
        where TBuilder : IApplicationBuilder<TBuilder>
    {
        private readonly IApplication<TBuilder> _application;

        public MiddlewareConfigurator(IServiceCollection services, IApplication<TBuilder> application) : base(services)
        {
            _application = application;
        }

        public IMiddlewareConfigurator<TBuilder> Use<TMiddleware>(string? alias, params object[]? arguments)
            where TMiddleware : class
        {
            _application.Builder.Services.AddTransient<TMiddleware>();
            _application.Builder.Services.Configure<IApplicationBuilder>(appBuilder =>
            {
                appBuilder.UseMiddleware<TMiddleware>(arguments);
            });
            return this;
        }

        public IMiddlewareBuilder<TBuilder, TMiddleware> UseMiddleware<TMiddleware>(string? alias, params object[]? arguments)
            where TMiddleware : class, IMiddleware
        {
            return new FluentMiddlewareBuilder<TBuilder, TMiddleware>(_application, _application.Builder.Services, alias ?? typeof(TMiddleware).Name);
        }

        public IApplication<TBuilder> Application => _application;
    }
}
