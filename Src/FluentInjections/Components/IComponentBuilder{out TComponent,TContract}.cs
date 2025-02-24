// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections.Components
{
    public interface IComponentBuilder<out TComponent, TContract> : IDisposable
        where TComponent : IComponent
    {
        IComponentBuilder<TComponent, TContract> To<TImplementation>() where TImplementation : class, TContract;
        IComponentBuilder<TComponent, TContract> ToSelf();
        IComponentBuilder<TComponent, TContract> UsingFactory(Func<IServiceProvider, CancellationToken, ValueTask<TContract>> factory);
        IComponentBuilder<TComponent, TContract> UsingInstance(TContract instance);
        IComponentBuilder<TComponent, TContract> AsSingleton();
        IComponentBuilder<TComponent, TContract> AsScoped();
        IComponentBuilder<TComponent, TContract> AsTransient();
        IComponentBuilder<TComponent, TContract> WithLifetime(ComponentLifetime lifetime);
        IComponentBuilder<TComponent, TContract> Configure(Action<TContract> configure);
        IComponentBuilder<TComponent, TContract> WithMetadata(string key, object? value);
        IComponentBuilder<TComponent, TContract> WithParameters(IDictionary<string, object?> parameters);
    }
}
