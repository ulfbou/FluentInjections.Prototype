// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Components;
using FluentInjections.DependencyInjection;
using FluentInjections.Validation;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FluentInjections.Builders;

internal abstract class ComponentBuilderBase<TComponent, TContract, TRegistration> : IComponentBuilder<TComponent, TContract>
    where TComponent : IComponent
    where TRegistration : IComponentRegistration<TComponent, TContract>
{
    protected readonly ILogger _logger;
    protected readonly IComponentResolver<TComponent> _innerResolver;
    protected readonly IServiceCollection _services;
    private readonly ILoggerFactory _loggerFactory;
    protected readonly IDisposable _scope;
    protected bool disposed;

    public abstract TRegistration Registration { get; }

    protected ComponentBuilderBase(IComponentResolver<TComponent> innerResolver, IServiceCollection services, ILoggerFactory loggerFactory)
    {
        Guard.NotNull(innerResolver, nameof(innerResolver));
        Guard.NotNull(services, nameof(services));
        Guard.NotNull(loggerFactory, nameof(loggerFactory));
        _innerResolver = innerResolver;
        _services = services;
        _loggerFactory = loggerFactory;
        _logger = loggerFactory.CreateLogger(GetType());
        _scope = _logger.BeginScope("ComponentBuilder<{Component}, {Contract}>", typeof(TComponent).Name, typeof(TContract).Name)!;
    }

    public IComponentBuilder<TComponent, TContract> To<TImplementation>() where TImplementation : class, TContract
    {
        _logger.LogInformation("Setting component implementation to {Implementation}.", typeof(TImplementation).Name);
        Registration.ResolutionType = typeof(TImplementation);
        return this;
    }

    public IComponentBuilder<TComponent, TContract> ToSelf()
    {
        _logger.LogInformation("Setting component implementation to self.");
        Registration.ResolutionType = typeof(TContract);
        return this;
    }

    public IComponentBuilder<TComponent, TContract> UsingInstance(TContract instance)
    {
        Guard.NotNull(instance, nameof(instance));
        _logger.LogInformation("Setting component instance.");
        Registration.Instance = instance;
        return this;
    }

    public IComponentBuilder<TComponent, TContract> UsingFactory(Func<IServiceProvider, CancellationToken, ValueTask<TContract>> factory)
    {
        Guard.NotNull(factory, nameof(factory));
        _logger.LogInformation("Setting component factory.");
        Registration.Factory = factory;
        return this;
    }

    public IComponentBuilder<TComponent, TContract> When(Func<IServiceProvider, bool> condition)
    {
        Guard.NotNull(condition, nameof(condition));
        _logger.LogInformation("Setting component condition.");
        Registration.Condition = condition;
        return this;
    }

    public IComponentBuilder<TComponent, TContract> AsSingleton()
    {
        _logger.LogInformation("Setting component lifetime to singleton.");
        Registration.Lifetime = ComponentLifetime.Singleton;
        return this;
    }

    public IComponentBuilder<TComponent, TContract> AsScoped()
    {
        _logger.LogInformation("Setting component lifetime to scoped.");
        Registration.Lifetime = ComponentLifetime.Scoped;
        return this;
    }

    public IComponentBuilder<TComponent, TContract> AsTransient()
    {
        _logger.LogInformation("Setting component lifetime to transient.");
        Registration.Lifetime = ComponentLifetime.Transient;
        return this;
    }

    public IComponentBuilder<TComponent, TContract> WithLifetime(ComponentLifetime lifetime)
    {
        _logger.LogInformation("Setting component lifetime to {Lifetime}.", lifetime);
        Registration.Lifetime = lifetime;
        return this;
    }

    public IComponentBuilder<TComponent, TContract> Configure(Action<TContract> configure)
    {
        Guard.NotNull(configure, nameof(configure));
        _logger.LogInformation("Setting component configure delegate.");
        Registration.Configure = configure;
        return this;
    }

    public IComponentBuilder<TComponent, TContract> WithMetadata(string key, object? value)
    {
        Guard.NotNullOrWhiteSpace(key, nameof(key));
        _logger.LogInformation("Setting component metadata.");
        Registration.Metadata[key] = value;
        return this;
    }

    public IComponentBuilder<TComponent, TContract> WithParameters(IDictionary<string, object?> parameters)
    {
        Guard.NotNull(parameters, nameof(parameters));
        _logger.LogInformation("Setting component parameters.");

        foreach (var (key, value) in parameters)
        {
            Registration.Parameters[key] = value;
        }

        return this;
    }

    public async ValueTask DisposeAsync()
    {
        if (!disposed)
        {
            _scope?.Dispose();
            var registry = await _innerResolver.ResolveSingleAsync<IComponentRegistry<IMiddlewareComponent>>(alias: null).ConfigureAwait(false);

            if (registry is not null && Registration is IComponentRegistration<IMiddlewareComponent, TContract> middlewareRegistration)
            {
                await registry.RegisterAsync(middlewareRegistration, CancellationToken.None).ConfigureAwait(false);
            }
        }
        disposed = true;
    }
}
