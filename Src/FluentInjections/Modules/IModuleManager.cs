﻿// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Configurators;

namespace FluentInjections.Modules
{
    public interface IModuleManager
    {
        Task ActivateModulesAsync<TConfigurator>(IServiceProvider serviceProvider, CancellationToken cancellationToken) where TConfigurator : class, IConfigurator;
        Task DiscoverModulesAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken);
        void RegisterModule(IModule module);
    }
}
