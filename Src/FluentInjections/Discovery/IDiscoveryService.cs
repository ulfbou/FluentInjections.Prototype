// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

namespace FluentInjections.Discovery
{
    public interface IDiscoveryService<TItem> : IServiceProvider, IAsyncServiceProvider
    {
        IServiceCollection Services { get; }

        Task<IServiceProvider> DiscoverAsync(CancellationToken cancellationToken = default);
    }
}
