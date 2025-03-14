// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using System;
using System.Reflection;

namespace FluentInjections
{
    public class DiscoveryConfiguration<TItem> : IDiscoveryConfiguration<TItem>
    {
        internal IServiceCollection Services { get; } = new ServiceCollection();
        public Assembly Assembly { get; private set; }
        public Func<Type, bool>? Filter { get; private set; }
        internal ILogger? Logger { get; private set; }

        public DiscoveryConfiguration(Assembly assembly)
        {
            Assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
        }

        public DiscoveryConfiguration<TItem> WithFilter(Func<Type, bool> filter)
        {
            Filter = filter;
            return this;
        }

        public DiscoveryConfiguration<TItem> WithLogger(ILogger logger)
        {
            Logger = logger;
            return this;
        }

        public DiscoveryConfiguration<TItem> ConfigureServices(Action<IServiceCollection> configureServices)
        {
            configureServices?.Invoke(Services);
            return this;
        }
    }
}
