// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Validation;

using Microsoft.Extensions.Logging;

namespace FluentInjections;

internal class MiddlewareBuilder<TBuilder, TContract, TRegistration>
    : ComponentBuilderBase<IMiddlewareComponent, TContract, TRegistration>
    , IMiddlewareBuilder<TBuilder, TContract>, IComponentBuilder<IMiddlewareComponent, TContract>
        where TBuilder : IApplicationBuilder<TBuilder>
        where TRegistration : IComponentRegistration<IMiddlewareComponent, TContract>
{
    public MiddlewareBuilder(IApplication<TBuilder> application, TRegistration registration, ILoggerFactory loggerFactory)
        : base(registration, loggerFactory)
    {
        Guard.NotNull(application, nameof(application));
        Application = application;
    }

    public IApplication<TBuilder> Application { get; }
}
