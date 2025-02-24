// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections.Modules
{
    public interface IModuleManagerHandler
    {
        Task ActivateMiddlewareModulesAsync<T>(IServiceProvider innerProvider, CancellationToken cancellationToken);
        Task InitializeModulesAsync(IServiceProvider innerProvider, CancellationToken cancellationToken);
    }
}
