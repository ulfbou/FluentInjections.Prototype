// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using System.Reflection;

namespace FluentInjections
{
    public interface IDiscoveryConfiguration<TItem>
    {
        Assembly Assembly { get; }
        Func<Type, bool>? Filter { get; }

        DiscoveryConfiguration<TItem> ConfigureServices(Action<IServiceCollection> configureServices);
        DiscoveryConfiguration<TItem> WithFilter(Func<Type, bool> filter);
        DiscoveryConfiguration<TItem> WithLogger(ILogger logger);
    }
}