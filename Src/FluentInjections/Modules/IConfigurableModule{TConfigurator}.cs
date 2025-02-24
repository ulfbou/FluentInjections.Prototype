// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Configurators;

namespace FluentInjections.Modules
{
    public interface IConfigurableModule<TConfigurator> : IModule where TConfigurator : IConfigurator
    {
        Task ConfigureAsync(TConfigurator configurator, CancellationToken cancellationToken = default);
    }
}
