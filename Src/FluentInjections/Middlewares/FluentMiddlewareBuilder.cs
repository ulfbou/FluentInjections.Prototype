// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Application;
using FluentInjections.Builders;
using FluentInjections.Components;
using FluentInjections.Configurators;
using FluentInjections.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FluentInjections.Middlewares
{
    internal sealed class FluentMiddlewareBuilder<TBuilder, TContract>
        : ComponentBuilderBase<IMiddlewareComponent, TContract, IMiddlewareRegistration<TContract>>
        , IMiddlewareBuilder<TBuilder, TContract>
        , IComponentBuilder<IMiddlewareComponent, TContract>
        where TBuilder : IApplicationBuilder<TBuilder>
    {
        private readonly MiddlewareRegistration<TContract> _registration;

        public IApplication<TBuilder> Application { get; }
        public override IMiddlewareRegistration<TContract> Registration => _registration;

        public FluentMiddlewareBuilder(IComponentResolver<IMiddlewareComponent> innerResolver, IApplication<TBuilder> application, ILoggerFactory loggerFactory, string alias)
            : base(innerResolver, application.Services, loggerFactory)
        {
            Application = application;
            _registration = new MiddlewareRegistration<TContract> { Alias = alias };
        }
    }
}
