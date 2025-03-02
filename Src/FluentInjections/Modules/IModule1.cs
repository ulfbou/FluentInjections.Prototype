// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Configuration;

namespace FluentInjections.Modules
{
    public interface IModule<TConfigurator> : IModule
        where TConfigurator : IConfigurator
    { }
}
