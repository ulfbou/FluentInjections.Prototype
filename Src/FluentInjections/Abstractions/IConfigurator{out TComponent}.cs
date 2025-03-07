// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions;
using FluentInjections.Registration;

namespace FluentInjections.Abstractions
{
    public interface IConfigurator<TComponent> : IConfigurator
        where TComponent : IComponent
    {
        ValueTask<IComponentBuilder<TComponent, TContract>> RegisterAsync<TContract>(string? alias = null, CancellationToken? cancellationToken = default);
        ValueTask<IComponentBuilder<TComponent, object>> RegisterAsync(string? alias = null, CancellationToken? cancellationToken = default);
    }
}
