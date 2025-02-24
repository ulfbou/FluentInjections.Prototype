// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Application;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FluentInjections.Injection;

public interface IBuilderRegistry
{
    void RegisterBuilder<TBuilder>(Func<string[]?, IServiceCollection?, ILoggerFactory?, TBuilder> factory)
        where TBuilder : IApplicationBuilder<TBuilder>;

    TBuilder CreateBuilder<TBuilder>(string[]? args, IServiceCollection? externalServices, ILoggerFactory? loggerFactory)
        where TBuilder : IApplicationBuilder<TBuilder>;
}
