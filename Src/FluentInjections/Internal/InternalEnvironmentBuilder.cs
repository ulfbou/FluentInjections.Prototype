// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections.Internal
{
    using FluentInjections.Abstractions;
    using FluentInjections.DependencyInjection;

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    using System;

    public class InternalEnvironmentBuilder : IInternalEnvironmentBuilder
    {
        private readonly IServiceCollection _services;

        public InternalEnvironmentBuilder()
        {
            _services = new ServiceCollection();
        }

        public InternalEnvironmentBuilder RegisterInternalServices()
        {
            _services.AddSingleton<ILoggerFactory, LoggerFactory>();
            // Add other core internal services as needed
            return this;
        }

        public InternalEnvironmentBuilder RegisterInternalRegistries()
        {
            _services.AddSingleton<IComponentRegistry<ILifecycleComponent>, ComponentRegistry<ILifecycleComponent>>();
            _services.AddSingleton<IComponentRegistry<IServiceComponent>, ComponentRegistry<IServiceComponent>>();
            return this;
        }

        public IServiceProvider Build()
        {
            return _services.BuildServiceProvider();
        }
    }
}