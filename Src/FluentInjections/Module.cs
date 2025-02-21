/// <summary>
/// 
/// </summary>
/// <typeparam name="TConfigurator"></typeparam>
// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Validation;

using Microsoft.AspNetCore.Builder;

namespace FluentInjections;

/// <summary>
/// Represents a module that can be configured by a configurator.
/// </summary>
/// <typeparam name="TConfigurator"></typeparam>
/// <typeparam name="TBuilder"></typeparam>
public abstract class Module<TConfigurator, TBuilder> : IConfigurableModule<TConfigurator>
    where TConfigurator : IConfigurator
    where TBuilder : class
{
    public abstract int Priority { get; }
    public Type ConfiguratorType { get; set; }
    public TBuilder? Builder { get; set; }

    public Module(TBuilder? builder = null)
    {
        ConfiguratorType = typeof(TConfigurator);
        Builder = builder;
    }

    /// <inheritdoc />
    public virtual bool CanHandle<T>() where T : IConfigurator => ConfiguratorType.IsAssignableFrom(typeof(T));

    /// <inheritdoc />
    public virtual bool CanHandle(Type configuratorType)
    {
        Guard.NotNull(configuratorType, nameof(configuratorType));
        return ConfiguratorType.IsAssignableFrom(configuratorType);
    }

    /// <inheritdoc />
    public abstract Task ConfigureAsync(TConfigurator configurator, CancellationToken? cancellationToken = null);
}
