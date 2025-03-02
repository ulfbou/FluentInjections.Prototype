// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions;
using FluentInjections.Registration;

namespace FluentInjections.Configuration
{
    public interface IConfigurator<out TComponent> : IConfigurator
        where TComponent : IComponent
    {
        IComponentBuilder<TComponent, TContract> RegisterAsync<TContract>(string? alias = null, CancellationToken? cancellationToken = null);
        IComponentBuilder<TComponent, object> Register(Type contractType, string? alias = null, CancellationToken? cancellationToken = null);
    }
}
