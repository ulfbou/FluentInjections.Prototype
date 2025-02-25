// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Application;
using FluentInjections.Validation;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using System.Collections.Concurrent;

namespace FluentInjections.Internal;

public class BuilderRegistry : IBuilderRegistry
{
    private readonly IDictionary<Type, Func<string[]?, IServiceCollection?, ILoggerFactory?, object>> _builderFactories = new ConcurrentDictionary<Type, Func<string[]?, IServiceCollection?, ILoggerFactory?, object>>();
    private readonly ILogger<BuilderRegistry> _logger;

    public BuilderRegistry(ILogger<BuilderRegistry> logger)
    {
        Guard.NotNull(logger, nameof(logger));
        _logger = logger;
    }

    public void RegisterBuilder<TBuilder>(Func<string[]?, IServiceCollection?, ILoggerFactory?, TBuilder> factory)
        where TBuilder : IApplicationBuilder<TBuilder>
    {
        _logger.LogDebug("Registering builder for type {BuilderName}.", typeof(TBuilder).FullName);
        _builderFactories[typeof(TBuilder)] = (args, services, loggerFactory) => factory(args, services, loggerFactory);
    }

    public TBuilder CreateBuilder<TBuilder>(string[]? args, IServiceCollection? externalServices, ILoggerFactory? loggerFactory)
        where TBuilder : IApplicationBuilder<TBuilder>
    {
        if (_builderFactories.TryGetValue(typeof(TBuilder), out var factory) && factory(args, externalServices, loggerFactory) is TBuilder builder)
        {
            _logger.LogDebug("Builder for type {BuilderName} created.", typeof(TBuilder).FullName);
            return builder;
        }

        throw new InvalidOperationException($"Builder for type {typeof(TBuilder).Name} is not registered.");
    }
}
