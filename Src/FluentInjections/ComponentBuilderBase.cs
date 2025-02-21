// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Validation;

using Microsoft.Extensions.Logging;

namespace FluentInjections;

internal abstract class ComponentBuilderBase<TComponent, TContract, TRegistration> : IComponentBuilder<TComponent, TContract>
    where TComponent : IComponent
    where TRegistration : IComponentRegistration<TComponent, TContract>
{
    protected readonly TRegistration _registration;
    protected readonly ILogger _logger;
    protected readonly IDisposable? _scope;
    protected bool disposed;

    public ComponentBuilderBase(TRegistration registration, ILoggerFactory loggerFactory)
    {
        Guard.NotNull(registration, nameof(registration));
        Guard.NotNull(loggerFactory, nameof(loggerFactory));

        _registration = registration;
        _logger = loggerFactory.CreateLogger(GetType());
        _scope = _logger.BeginScope("ComponentBuilder<{Component}, {Contract}>", typeof(TComponent).Name, typeof(TContract).Name);
    }

    public IComponentBuilder<TComponent, TContract> To<TImplementation>() where TImplementation : class, TContract
    {
        _logger.LogInformation("Setting component implementation to {Implementation}.", typeof(TImplementation).Name);
        _registration.ResolutionType = typeof(TImplementation);
        return this;
    }

    public IComponentBuilder<TComponent, TContract> ToSelf()
    {
        _logger.LogInformation("Setting component implementation to self.");
        _registration.ResolutionType = typeof(TContract);
        return this;
    }

    public IComponentBuilder<TComponent, TContract> UsingInstance(TContract instance)
    {
        Guard.NotNull(instance, nameof(instance));
        _logger.LogInformation("Setting component instance.");
        _registration.Instance = instance;
        return this;
    }

    public IComponentBuilder<TComponent, TContract> UsingFactory(Func<IServiceProvider, TContract> factory)
    {
        Guard.NotNull(factory, nameof(factory));
        _logger.LogInformation("Setting component factory.");
        _registration.Factory = factory;
        return this;
    }

    public IComponentBuilder<TComponent, TContract> When(Func<IServiceProvider, bool> condition)
    {
        Guard.NotNull(condition, nameof(condition));
        _logger.LogInformation("Setting component condition.");
        _registration.Condition = condition;
        return this;
    }

    public IComponentBuilder<TComponent, TContract> AsSingleton()
    {
        _logger.LogInformation("Setting component lifetime to singleton.");
        _registration.Lifetime = ComponentLifetime.Singleton;
        return this;
    }

    public IComponentBuilder<TComponent, TContract> AsScoped()
    {
        _logger.LogInformation("Setting component lifetime to scoped.");
        _registration.Lifetime = ComponentLifetime.Scoped;
        return this;
    }

    public IComponentBuilder<TComponent, TContract> AsTransient()
    {
        _logger.LogInformation("Setting component lifetime to transient.");
        _registration.Lifetime = ComponentLifetime.Transient;
        return this;
    }

    public IComponentBuilder<TComponent, TContract> WithLifetime(ComponentLifetime lifetime)
    {
        _logger.LogInformation("Setting component lifetime to {Lifetime}.", lifetime);
        _registration.Lifetime = lifetime;
        return this;
    }

    public IComponentBuilder<TComponent, TContract> Configure(Action<TContract> configure)
    {
        Guard.NotNull(configure, nameof(configure));
        _logger.LogInformation("Setting component configure delegate.");
        _registration.Configure = configure;
        return this;
    }

    public IComponentBuilder<TComponent, TContract> WithMetadata(string key, object? value)
    {
        Guard.NotNullOrWhiteSpace(key, nameof(key));
        _logger.LogInformation("Setting component metadata.");
        _registration.Metadata[key] = value;
        return this;
    }

    public IComponentBuilder<TComponent, TContract> WithParameters(object parameters)
    {
        Guard.NotNull(parameters, nameof(parameters));
        _logger.LogInformation("Setting component parameters.");
        _registration.Parameters = parameters;
        return this;
    }

    public void Dispose()
    {
        if (disposed)
        {
            return;
        }

        _scope?.Dispose();
        disposed = true;
    }
}
