// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.DependencyInjection;
using FluentInjections.Validation;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FluentInjections;

internal class ServiceConfigurator : ComponentConfiguratorBase<IServiceComponent>, IServiceConfigurator
{
    private IServiceCollection _services;

    public ServiceConfigurator(IServiceCollection serviceCollection, IComponentRegistry<IServiceComponent> innerRegistry, ILoggerFactory loggerFactory)
        : base(innerRegistry, loggerFactory)
    {
        _services = serviceCollection ?? throw new ArgumentNullException(nameof(serviceCollection));
    }

    public IServiceCollection Services => _services;

    protected override IComponentBuilder<IServiceComponent, TContract> CreateBuilder<TContract>(IComponentRegistration<IServiceComponent, TContract> registration)
    {
        return new ServiceBuilder<TContract, IComponentRegistration<IServiceComponent, TContract>>(registration, _loggerFactory);
    }
    protected override IComponentRegistration<IServiceComponent, object> CreateRegistration(Type contractType, string alias)
    {
        return new ServiceRegistration<object>
        {
            ContractType = contractType,
            Alias = alias
        };
    }

    protected override IComponentRegistration<IServiceComponent, TContract> CreateRegistration<TContract>(string alias)
    {
        return new ServiceRegistration<TContract>
        {
            ContractType = typeof(TContract),
            Alias = alias
        };
    }
}
