// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Application;
using FluentInjections.Components;

using Microsoft.Extensions.DependencyInjection;

namespace FluentInjections.Middlewares
{
    public class FluentMiddlewareBuilder<TBuilder, TContract> : FluentComponentBuilder<IMiddlewareComponent, TContract>, IMiddlewareBuilder<TBuilder, TContract>
       where TBuilder : IApplicationBuilder<TBuilder>
    {
        private readonly IApplication<TBuilder> _application;

        public FluentMiddlewareBuilder(IApplication<TBuilder> application, IServiceCollection services, string alias) : base(services, alias)
        {
            _application = application;
        }

        public IApplication<TBuilder> Application => _application;
    }
}
