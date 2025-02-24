// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Components;
using FluentInjections.DependencyInjection;
using FluentInjections.Validation;

using System.Collections.Concurrent;

namespace FluentInjections.Descriptors;

public record ComponentDescriptor<TComponent, TContract> : IComponentDescriptor<TComponent, TContract>, IComponentCreationDescriptor<TComponent, TContract>
    where TComponent : IComponent
{
    public string Alias { get; init; }
    public ComponentLifetime Lifetime { get; init; }
    public Type ContractType { get; init; }
    public Type? ResolutionType { get; init; }
    public IReadOnlyDictionary<string, object?> Metadata { get; init; }
    public object Parameters { get; init; }
    public Func<IServiceProvider, bool>? Condition { get; init; }
    public TContract? Instance { get; init; }
    public Func<IServiceProvider, CancellationToken?, ValueTask<TContract>>? Factory { get; init; }
    public Action<TContract>? Configure { get; init; }
    public Func<IServiceProvider, TContract>? Decorator { get; init; }

    public IDictionary<string, object?> Dependencies { get; init; }

    Func<IServiceProvider, CancellationToken?, ValueTask<TContract>>? IComponentDescriptor<TComponent, TContract>.Factory => throw new NotImplementedException();

    public ComponentDescriptor(
        string Alias,
        ComponentLifetime Lifetime = ComponentLifetime.Transient,
        Type? ResolutionType = null,
        IReadOnlyDictionary<string, object?>? Metadata = null,
        object? Parameters = null,
        Func<bool>? Condition = null,
        TContract? Instance = default,
        Func<IServiceProvider, CancellationToken?, ValueTask<TContract>>? Factory = null,
        Action<TContract>? Configure = null,
        Func<IServiceProvider, TContract>? Decorator = null)
    {
        Guard.NotNull(Alias, nameof(Alias));
        Guard.NotNull(Lifetime, nameof(Lifetime));
        Guard.NotNull(Metadata, nameof(Metadata));
        Guard.NotNull(Parameters, nameof(Parameters));

        this.Alias = Alias;
        this.Lifetime = Lifetime;
        this.ContractType = typeof(TContract);
        this.Metadata = Metadata is null ? new ConcurrentDictionary<string, object?>() : new ConcurrentDictionary<string, object?>(Metadata);
        this.Parameters = Parameters is null ? new object[] { } : Parameters;
        this.Condition = Condition is null ? null : new Func<IServiceProvider, bool>(provider => Condition());
        this.Instance = Instance;
        this.Factory = Factory;
        this.Configure = Configure;
        this.Decorator = Decorator;
        this.Dependencies = new ConcurrentDictionary<string, object?>();
    }

    public ComponentDescriptor(IComponentRegistration<TComponent, TContract> registration)
    {
        this.Alias = registration.Alias;
        this.Lifetime = registration.Lifetime;
        this.ContractType = registration.ContractType;
        this.Parameters = registration.Parameters ?? new Dictionary<string, object?>();
        this.Metadata = registration.Metadata.AsReadOnly();
        this.Condition = registration.Condition;

        if (registration is not IComponentCreationRegistration<TComponent, TContract> creationRegistration)
        {
            throw new InvalidCastException(nameof(creationRegistration));
        }

        this.Instance = creationRegistration.Instance;
        this.Factory = creationRegistration.Factory;
        this.Configure = creationRegistration.Configure;
        this.Decorator = creationRegistration.Decorator;
        this.Dependencies = new ConcurrentDictionary<string, object?>(); // TODO: Add dependencies to registration
    }
}
