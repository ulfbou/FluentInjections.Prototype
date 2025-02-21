// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Http;

namespace FluentInjections;

public interface IMiddlewareConfigurator<TBuilder> : IConfigurator<IMiddlewareComponent>
    where TBuilder : IApplicationBuilder<TBuilder>
{
    IMiddlewareConfigurator<TBuilder> Use<TMiddleware>(string? alias, params object[]? arguments)
            where TMiddleware : class;
    IMiddlewareBuilder<TBuilder, TMiddleware> UseMiddleware<TMiddleware>(string? alias, params object[]? arguments)
        where TMiddleware : class, IMiddleware;

    IApplication<TBuilder> Application { get; }
}
