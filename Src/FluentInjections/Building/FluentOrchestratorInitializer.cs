// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.DependencyInjection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FluentInjections
{
    internal class FluentOrchestratorInitializer<T, TApplication>
    {
        private IConfiguration configuration;
        private ILoggerFactory loggerFactory;
        private IComponentResolverProvider resolverProvider;

        public FluentOrchestratorInitializer(IConfiguration configuration, ILoggerFactory loggerFactory, IComponentResolverProvider resolverProvider)
        {
            this.configuration = configuration;
            this.loggerFactory = loggerFactory;
            this.resolverProvider = resolverProvider;
        }

        internal Task InitializeAsync(CancellationToken cancellationToken) => throw new NotImplementedException();
    }
}