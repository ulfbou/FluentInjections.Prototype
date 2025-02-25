// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Application;
using FluentInjections.Configurators;

using Microsoft.AspNetCore.Http;

namespace FluentInjections.Middlewares
{
    public interface IMiddlewareConfigurator<TBuilder> : IConfigurator<IMiddlewareComponent>
        where TBuilder : IApplicationBuilder<TBuilder>
    {
        IApplication<TBuilder> Application { get; }
    }
}
