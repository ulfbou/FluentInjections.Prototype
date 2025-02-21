﻿// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections;

public interface IComponentCreationRegistration<out TComponent, TContract>
    where TComponent : IComponent
{
    TContract? Instance { get; set; }
    Func<IServiceProvider, TContract>? Factory { get; set; }
    Action<TContract>? Configure { get; set; }
    Func<IServiceProvider, TContract>? Decorator { get; set; }
}
