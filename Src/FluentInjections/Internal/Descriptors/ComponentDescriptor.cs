// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions;
using FluentInjections.DependencyInjection;
using FluentInjections.Exceptions;
using FluentInjections.Validation;

namespace FluentInjections.Internal.Descriptors
{
    internal record ComponentDescriptor<TComponent, TContract> : IComponentDescriptor<TComponent, TContract> where TComponent : IComponent
    {
        public ComponentDescriptor(IComponentRegistration<TComponent, TContract> registration)
        {
            Guard.NotNull(registration, nameof(registration));

            if (registration.ContractType != typeof(TContract))
            {
                throw new ComponentContractMismatchException(registration.ContractType, typeof(TContract));
            }

            Alias = registration.Alias;
            ContractType = registration.ContractType;
            Lifetime = registration.Lifetime;
            ResolutionType = registration.ResolutionType;
            Instance = registration.Instance;
            Factory = registration.Factory;
            Configure = registration.Configure;
            Metadata = registration.Metadata.AsReadOnly();
            Parameters = registration.Parameters;
            Condition = registration.Condition;
        }

        public string Alias { get; init; }
        public Type ContractType { get; init; }
        public ComponentLifetime Lifetime { get; init; }
        public Type? ResolutionType { get; init; }
        public TContract? Instance { get; init; }
        public Func<IComponentResolver<TComponent>, CancellationToken, ValueTask<TContract>>? Factory { get; init; }
        public Func<TContract, CancellationToken, ValueTask>? Configure { get; init; }
        public IReadOnlyDictionary<string, object?> Metadata { get; init; }
        public object? Parameters { get; init; }
        public Func<IComponentResolver<TComponent>, CancellationToken, ValueTask<bool>>? Condition { get; init; }
    }
}
