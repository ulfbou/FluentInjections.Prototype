// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections;
using FluentInjections.Components;

namespace FluentInjections.Configurators
{
    public interface IConfigurator<TComponent> : IConfigurator
        where TComponent : IComponent
    {
        ValueTask<IComponentBuilder<TComponent, TContract>> RegisterAsync<TContract>(string? alias = null);
        ValueTask<IComponentBuilder<TComponent, object>> RegisterAsync(Type contractType, string? alias = null);
    }
}
