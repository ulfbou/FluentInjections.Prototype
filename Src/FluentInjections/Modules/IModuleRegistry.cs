// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Configurators;

namespace FluentInjections.Modules
{
    public interface IModuleRegistry
    {
        Task ApplyAsync<TConfigurator>(TConfigurator configurator, CancellationToken cancellationToken = default)
            where TConfigurator : IConfigurator;

        Task InitializeAsync(CancellationToken cancellationToken = default);

        Task RegisterAsync(Type configuratorType, IModule moduleInstance, CancellationToken cancellationToken = default);

        Task RegisterAsync<TConfigurator>(IConfigurableModule<TConfigurator> module, CancellationToken cancellationToken = default)
            where TConfigurator : IConfigurator;

        Task RegisterAsync<TModule, TConfigurator>(Func<TModule> factory, Action<TModule> configure, CancellationToken cancellationToken = default)
            where TModule : IConfigurableModule<TConfigurator>
            where TConfigurator : IConfigurator;

        Task RegisterAsync<TModule, TConfigurator>(TModule module, CancellationToken cancellationToken = default)
            where TModule : IConfigurableModule<TConfigurator>
            where TConfigurator : IConfigurator;

        Task UnregisterAsync<TConfigurator>(Type moduleType, IConfigurableModule<TConfigurator> module, CancellationToken cancellationToken = default)
            where TConfigurator : IConfigurator;

        Task UnregisterAsync<TModule, TConfigurator>(TModule module, CancellationToken cancellationToken = default)
            where TModule : IConfigurableModule<TConfigurator>
            where TConfigurator : IConfigurator;
    }
}
