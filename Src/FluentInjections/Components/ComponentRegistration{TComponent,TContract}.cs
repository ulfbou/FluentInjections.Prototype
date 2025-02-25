// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.DependencyInjection;

namespace FluentInjections.Components
{
    internal abstract class ComponentRegistration<TComponent, TContract> : IComponentRegistration<TComponent, TContract>
        where TComponent : IComponent
    {
        public Type ContractType { get; } = typeof(TContract);

        public required string Alias { get; set; }
        public virtual Type? ResolutionType { get; set; }
        public virtual Func<IServiceProvider, CancellationToken, ValueTask<TContract>>? Factory { get; set; }
        public virtual TContract? Instance { get; set; }
        public virtual ComponentLifetime Lifetime { get; set; } = ComponentLifetime.Transient;
        public virtual Action<TContract> Configure { get; set; } = _ => { };
        public virtual Func<IServiceProvider, bool>? Condition { get; set; }
        public IDictionary<string, object?> Metadata { get; } = new Dictionary<string, object?>();
        public IDictionary<string, object?> Parameters { get; } = new Dictionary<string, object?>();
        public IDictionary<string, Type> Dependencies { get; } = new Dictionary<string, Type>();
        public virtual Func<IServiceProvider, CancellationToken, ValueTask<TContract>>? Decorator { get; set; }

        public virtual ServiceDescriptor BuildDescriptor()
        {
            if (Instance is not null)
            {
                return new ServiceDescriptor(ContractType, Instance);
            }

            if (Factory is not null)
            {
                return new ServiceDescriptor(ContractType, serviceProvider => Factory(serviceProvider, default).AsTask().Result!, Lifetime.ToServiceLifetime());
            }

            if (ResolutionType is not null)
            {
                return new ServiceDescriptor(ContractType, ResolutionType, Lifetime.ToServiceLifetime());
            }

            throw new InvalidOperationException("Component registration is incomplete. Please provide an instance, factory, or resolution type.");
        }
    }
}
