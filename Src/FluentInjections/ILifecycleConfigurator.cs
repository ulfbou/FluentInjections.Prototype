// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections;

public interface ILifecycleConfigurator : IConfigurator<ILifecycleComponent>
{
    Task<IComponentBuilder<ILifecycleComponent, TContract>> RegisterAsync<TContract>(string? alias = null);
    Task<IComponentBuilder<ILifecycleComponent, object>> RegisterAsync(Type contractType, string? alias = null);
}
