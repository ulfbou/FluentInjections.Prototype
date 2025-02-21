// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FluentInjections
{
    internal class LifecycleConfigurator : ComponentConfiguratorBase<ILifecycleComponent>, ILifecycleConfigurator
    {
        public LifecycleConfigurator(IComponentRegistry<ILifecycleComponent> registry, ILoggerFactory loggerFactory)
            : base(registry, loggerFactory)
        { }

        public override Task RegisterAsync(CancellationToken? cancellationToken = null)
        {
            throw new NotImplementedException();
        }

        public Task<IComponentBuilder<ILifecycleComponent, TContract>> RegisterAsync<TContract>(string? alias = null) => throw new NotImplementedException();
        public Task<IComponentBuilder<ILifecycleComponent, object>> RegisterAsync(Type contractType, string? alias = null) => throw new NotImplementedException();
        protected override IComponentBuilder<ILifecycleComponent, TContract> CreateBuilder<TContract>(IComponentRegistration<ILifecycleComponent, TContract> registration) => throw new NotImplementedException();
        protected override IComponentRegistration<ILifecycleComponent, object> CreateRegistration(Type componentType, string alias) => throw new NotImplementedException();
        protected override IComponentRegistration<ILifecycleComponent, TContract> CreateRegistration<TContract>(string alias) => throw new NotImplementedException();
    }
}
