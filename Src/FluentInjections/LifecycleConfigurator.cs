// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FluentInjections;

internal class LifecycleConfigurator : ComponentConfiguratorBase<ILifecycleComponent>, ILifecycleConfigurator
{
    public LifecycleConfigurator(IComponentRegistry<ILifecycleComponent> registry, ILoggerFactory loggerFactory)
        : base(registry, loggerFactory)
    { }

    protected override IComponentBuilder<ILifecycleComponent, TContract> CreateBuilder<TContract>(IComponentRegistration<ILifecycleComponent, TContract> registration)
    {
        return new LifecycleBuilder<TContract, IComponentRegistration<ILifecycleComponent, TContract>>(registration, _loggerFactory);
    }

    protected override IComponentRegistration<ILifecycleComponent, object> CreateRegistration(Type componentType, string alias)
    {
        return new LifecycleRegistration<object>
        {
            ContractType = componentType,
            Alias = alias
        };
    }

    protected override IComponentRegistration<ILifecycleComponent, TContract> CreateRegistration<TContract>(string alias)
    {
        return new LifecycleRegistration<TContract>
        {
            ContractType = typeof(TContract),
            Alias = alias
        };
    }
}
