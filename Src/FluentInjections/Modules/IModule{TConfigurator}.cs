// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions;

namespace FluentInjections.Modules
{
    public interface IModule<TConfigurator> : IModule
        where TConfigurator : IConfigurator
    { }
}
