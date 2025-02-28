// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Application;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FluentInjections.Abstractions.Factories
{
    public interface IBuilderFactory<TBuilder>
        where TBuilder : class, IApplicationBuilder<TBuilder>
    {
        Func<string[]?, IServiceCollection?, ILoggerFactory?, TBuilder> CreateBuilder(ILoggerFactory loggerFactory);
    }
}
