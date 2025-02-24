// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Configurators;

namespace FluentInjections.Modules
{
    public abstract class ConfigurableModuleBase<TConfigurator> : ModuleBase, IConfigurableModule<TConfigurator>
        where TConfigurator : IConfigurator
    {
        public abstract Task ConfigureAsync(TConfigurator configurator, CancellationToken cancellationToken = default);
    }
}
