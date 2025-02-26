// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections.Configurators
{
    public abstract class ConfiguratorBase : IConfigurator
    {
        public virtual Task RegisterAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}
