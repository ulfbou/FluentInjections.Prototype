// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FluentInjections
{
    public interface IFluentInjectionsConfiguration<TItem>
    {
        DiscoveryConfiguration<TItem> ConfigureServices(Action<IServiceCollection> configureServices);
        DiscoveryConfiguration<TItem> WithItemFilter(Func<Type, bool> filter);
        DiscoveryConfiguration<TItem> WithLoggerFactory(ILogger logger);
    }
}