// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Application;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FluentInjections.Injection;

public class BuilderRegistry : IBuilderRegistry
{
    private readonly Dictionary<Type, Func<string[]?, IServiceCollection?, ILoggerFactory?, object>> _builderFactories = new Dictionary<Type, Func<string[]?, IServiceCollection?, ILoggerFactory?, object>>();

    public void RegisterBuilder<TBuilder>(Func<string[]?, IServiceCollection?, ILoggerFactory?, TBuilder> factory)
        where TBuilder : IApplicationBuilder<TBuilder>
    {
        _builderFactories[typeof(TBuilder)] = (args, services, loggerFactory) => factory(args, services, loggerFactory);
    }

    public TBuilder CreateBuilder<TBuilder>(string[]? args, IServiceCollection? externalServices, ILoggerFactory? loggerFactory)
        where TBuilder : IApplicationBuilder<TBuilder>
    {
        if (_builderFactories.TryGetValue(typeof(TBuilder), out var factory))
        {
            return (TBuilder)factory(args, externalServices, loggerFactory);
        }

        throw new InvalidOperationException($"Builder for type {typeof(TBuilder).Name} is not registered.");
    }
}
