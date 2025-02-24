// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.DependencyInjection;

namespace FluentInjections.Components
{
    public class ComponentRegistration<TComponent, TContract> : IComponentRegistration<TComponent, TContract>
        where TComponent : IComponent
    {
        public required Type ContractType { get; set; }
        public required string Alias { get; set; }
        public Type? ResolutionType { get; set; }
        public Func<IServiceProvider, CancellationToken, ValueTask<TContract>>? Factory { get; set; }
        public TContract? Instance { get; set; }
        public ComponentLifetime Lifetime { get; set; } = ComponentLifetime.Transient;
        public Action<TContract>? Configure { get; set; }
        public Func<IServiceProvider, bool>? Condition { get; set; }
        public IDictionary<string, object?> Metadata { get; } = new Dictionary<string, object?>();
        public IDictionary<string, object?> Parameters { get; set; } = new Dictionary<string, object?>();
        public IDictionary<string, Type> Dependencies { get; set; } = new Dictionary<string, Type>();
        public Func<IServiceProvider, CancellationToken, ValueTask<TContract>>? Decorator { get; set; }
        public ServiceDescriptor Descriptor
        {
            get
            {
                if (_descriptor is null)
                {
                    _descriptor = ToServiceDescriptor();
                }

                return _descriptor;
            }
        }


        private ServiceDescriptor? _descriptor;

        public ServiceDescriptor ToServiceDescriptor()
        {
            ServiceDescriptor? descriptor = _descriptor;

            if (descriptor is not null)
            {
                return descriptor;
            }

            if (Instance != null)
            {
                descriptor = new ServiceDescriptor(ContractType, Instance);
            }
            else if (Factory != null)
            {
                descriptor = new ServiceDescriptor(ContractType, serviceProvider => Factory(serviceProvider, default).AsTask().Result!, Lifetime.ToServiceLifetime());
            }
            else if (ResolutionType != null)
            {
                descriptor = new ServiceDescriptor(ContractType, ResolutionType, Lifetime.ToServiceLifetime());
            }
            else
            {
                throw new InvalidOperationException("Component registration is incomplete.");
            }

            return descriptor;
        }
    }
}
