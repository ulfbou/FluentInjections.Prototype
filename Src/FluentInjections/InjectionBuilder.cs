// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Application;
using FluentInjections.Internal;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FluentInjections
{
    public static class InjectionBuilder
    {
        private static ILoggerFactory _loggerFactory = new InternalLoggerFactory();
        private static ILogger<BuilderRegistry> _registryLogger;
        private static BuilderRegistry _registry;

        static InjectionBuilder()
        {
            _registryLogger = _loggerFactory.CreateLogger<BuilderRegistry>();
            _registry = new BuilderRegistry(_registryLogger);
        }

        public static IInjectionBuilder<TBuilder> For<TBuilder>(string[]? args = null, IServiceCollection? externalServices = null)
            where TBuilder : class, IInjectionBuilder<TBuilder>, IApplicationBuilder<TBuilder>
        {
            externalServices ??= new ServiceCollection();
            var discoverer = new BuilderDiscoverer<TBuilder>(_loggerFactory, _registry);

            discoverer.DiscoverAndRegisterBuilders();
            return _registry.CreateBuilder<TBuilder>(args, externalServices, _loggerFactory);
        }
    }
}
