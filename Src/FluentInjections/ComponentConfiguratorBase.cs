// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.DependencyInjection;
using FluentInjections.Validation;
using FluentInjections;
using Microsoft.Extensions.Logging;

internal abstract class ComponentConfiguratorBase<TComponent> : IConfigurator<TComponent>
    where TComponent : IComponent
{
    protected static readonly CancellationToken _defaultCancellationToken;
    protected List<IComponentRegistration<TComponent>> _registrations = new();
    protected readonly IComponentRegistry<TComponent> _registry;
    protected readonly ILogger _logger;
    protected readonly ILoggerFactory _loggerFactory;

    static ComponentConfiguratorBase()
    {
        _defaultCancellationToken = new CancellationToken();
    }

    public ComponentConfiguratorBase(IComponentRegistry<TComponent> registry, ILoggerFactory loggerFactory)
    {
        Guard.NotNull(registry, nameof(registry));
        Guard.NotNull(loggerFactory, nameof(loggerFactory));
        _registry = registry;
        _logger = loggerFactory.CreateLogger(GetType());
        _loggerFactory = loggerFactory;
    }

    public IComponentBuilder<TComponent, TContract> Register<TContract>(string alias)
    {
        alias ??= typeof(TContract).FullName ?? typeof(TContract).Name;
        var registration = CreateRegistration<TContract>(alias!);
        _registrations.Add(registration);
        return CreateBuilder(registration);
    }

    public IComponentBuilder<TComponent, object> Register(Type contractType, string alias)
    {
        Guard.NotNull(contractType, nameof(contractType));
        IComponentRegistration<TComponent, object> registration = CreateRegistration(contractType, alias!);
        _registrations.Add(registration);
        return CreateBuilder(registration);
    }

    public Task<IComponentBuilder<TComponent, TContract>> RegisterAsync<TContract>(string? alias = null)
    {
        alias ??= typeof(TContract).FullName ?? typeof(TContract).Name;
        var registration = CreateRegistration<TContract>(alias!);
        _registrations.Add(registration);
        return Task.FromResult(CreateBuilder(registration));
    }

    public Task<IComponentBuilder<TComponent, object>> RegisterAsync(Type contractType, string? alias = null)
    {
        Guard.NotNull(contractType, nameof(contractType));
        IComponentRegistration<TComponent, object> registration = CreateRegistration(contractType, alias!);
        _registrations.Add(registration);
        return Task.FromResult(CreateBuilder(registration));
    }


    protected abstract IComponentBuilder<TComponent, TContract> CreateBuilder<TContract>(IComponentRegistration<TComponent, TContract> registration);
    protected abstract IComponentRegistration<TComponent, object> CreateRegistration(Type contractType, string alias);
    protected abstract IComponentRegistration<TComponent, TContract> CreateRegistration<TContract>(string alias);

    public virtual async Task RegisterAsync(CancellationToken? cancellationToken = null)
    {
        var token = cancellationToken ?? _defaultCancellationToken;

        foreach (var registration in _registrations)
        {
            _logger.LogDebug("Registering component {Alias} for contract {ContractType}", registration.Alias, registration.ContractType?.FullName ?? "object");

            await _registry.RegisterAsync(registration.ComponentDescriptor, token);
        }
    }
}
