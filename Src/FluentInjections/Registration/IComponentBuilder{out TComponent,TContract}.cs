// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions;
using FluentInjections.DependencyInjection;

namespace FluentInjections.Registration
{
    public interface IComponentBuilder<TComponent, TContract> : IComponentBuilder<TComponent>, IDisposable
        where TComponent : IComponent
    {
        IComponentBuilder<TComponent, TContract> To<TImplementation>() where TImplementation : class, TContract;
        IComponentBuilder<TComponent, TContract> ToSelf();
        IComponentBuilder<TComponent, TContract> UsingFactory(Func<IComponentResolver<TComponent>, CancellationToken, ValueTask<TContract>> factory);
        IComponentBuilder<TComponent, TContract> UsingInstance(TContract instance);
        IComponentBuilder<TComponent, TContract> AsSingleton();
        IComponentBuilder<TComponent, TContract> AsScoped();
        IComponentBuilder<TComponent, TContract> AsTransient();
        IComponentBuilder<TComponent, TContract> WithLifetime(ComponentLifetime lifetime);
        IComponentBuilder<TComponent, TContract> WithMetadata(string key, object? value);
        IComponentBuilder<TComponent, TContract> WithParameters(object parameters);
        IComponentBuilder<TComponent, TContract> Configure(Func<TContract, CancellationToken, ValueTask> configure);
    }
}
