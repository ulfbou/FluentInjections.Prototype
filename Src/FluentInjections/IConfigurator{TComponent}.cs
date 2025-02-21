﻿// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.


namespace FluentInjections;

public interface IConfigurator<out TComponent> : IConfigurator
    where TComponent : IComponent
{
    IComponentBuilder<TComponent, TContract> Register<TContract>(string alias);
    IComponentBuilder<TComponent, object> Register(Type contractType, string alias);
}
