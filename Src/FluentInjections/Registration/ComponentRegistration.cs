// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.


using FluentInjections.Abstractions;
using FluentInjections.DependencyInjection;
using FluentInjections.Internal.Core;
using FluentInjections.Internal.Descriptors;

namespace FluentInjections.Registration
{
    internal class ComponentRegistration<TComponent, TContract> : IComponentRegistration<TComponent, TContract>
        where TComponent : IComponent
    {
        public TContract? Instance { get; set; }
        public Func<IComponentResolver<TComponent>, CancellationToken, ValueTask<TContract>>? Factory { get; set; }
        public Func<TContract, CancellationToken, ValueTask>? Configure { get; set; }

        public required string Alias { get; set; }

        public Type ContractType { get; set; } = typeof(TContract);
        public Type? ResolutionType { get; set; }
        public ComponentLifetime Lifetime { get; set; }

        public IDictionary<string, object?> Metadata { get; } = new Dictionary<string, object?>();

        public object? Parameters { get; set; }
        public Func<IComponentResolver<TComponent>, CancellationToken, ValueTask<bool>>? Condition { get; set; }

        public IComponentDescriptor<TComponent, TContract> CreateDescriptor(string alias)
        {
            return new ComponentDescriptor<TComponent, TContract>(this);
        }

        public void Dispose()
        {
            if (Instance is IDisposable disposable)
            {
                disposable.Dispose();
            }

            Instance = default;
            Metadata.Clear();

            if (Parameters is IDisposable disposableParameters)
            {
                disposableParameters.Dispose();
            }
        }
    }
}
