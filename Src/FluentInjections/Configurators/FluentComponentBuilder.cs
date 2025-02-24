// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.DependencyInjection;

namespace FluentInjections.Components
{
    public class FluentComponentBuilder<TComponent, TContract> : IComponentBuilder<TComponent, TContract>
        where TComponent : IComponent
    {
        private readonly IServiceCollection _services;
        private readonly ComponentRegistration<TComponent, TContract> _registration;
        private bool _disposed = false;

        public FluentComponentBuilder(IServiceCollection services, string alias)
        {
            _services = services;
            _registration = new ComponentRegistration<TComponent, TContract> { Alias = alias, ContractType = typeof(TContract) };
        }

        public IComponentBuilder<TComponent, TContract> To<TImplementation>() where TImplementation : class, TContract
        {
            _registration.ResolutionType = typeof(TImplementation);
            return this;
        }

        public IComponentBuilder<TComponent, TContract> ToSelf()
        {
            _registration.ResolutionType = typeof(TContract);
            return this;
        }

        public IComponentBuilder<TComponent, TContract> UsingFactory(Func<IServiceProvider, CancellationToken, ValueTask<TContract>> factory)
        {
            _registration.Factory = factory;
            return this;
        }

        public IComponentBuilder<TComponent, TContract> UsingInstance(TContract instance)
        {
            _registration.Instance = instance;
            return this;
        }

        public IComponentBuilder<TComponent, TContract> AsSingleton()
        {
            _registration.Lifetime = ComponentLifetime.Singleton;
            return this;
        }

        public IComponentBuilder<TComponent, TContract> AsScoped()
        {
            _registration.Lifetime = ComponentLifetime.Scoped;
            return this;
        }

        public IComponentBuilder<TComponent, TContract> AsTransient()
        {
            _registration.Lifetime = ComponentLifetime.Transient;
            return this;
        }

        public IComponentBuilder<TComponent, TContract> WithLifetime(ComponentLifetime lifetime)
        {
            _registration.Lifetime = lifetime;
            return this;
        }

        public IComponentBuilder<TComponent, TContract> Configure(Action<TContract> configure)
        {
            _registration.Configure = configure;
            return this;
        }

        public IComponentBuilder<TComponent, TContract> WithMetadata(string key, object? value)
        {
            _registration.Metadata[key] = value;
            return this;
        }

        public IComponentBuilder<TComponent, TContract> WithParameters(IDictionary<string, object> parameters)
        {
            _registration.Parameters = parameters;
            return this;
        }

        public IComponentBuilder<TComponent, TContract> WithDependency<TDependency>(string alias)
        {
            _registration.Dependencies[alias] = typeof(TDependency);
            return this;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _services.Add(_registration.ToServiceDescriptor());
                _disposed = true;
            }
        }
    }
}
