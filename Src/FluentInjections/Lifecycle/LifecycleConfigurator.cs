// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Components;
using FluentInjections.Configurators;
using FluentInjections.Validation;

using Microsoft.Extensions.DependencyInjection;

namespace FluentInjections.Lifecycle
{
    public class LifecycleConfigurator : ConfiguratorBase<ILifecycleComponent>, ILifecycleConfigurator
    {
        public LifecycleConfigurator(IServiceCollection services) : base(services) { }

        public override IComponentBuilder<ILifecycleComponent, TContract> Register<TContract>(string? alias = null)
        {
            return new LifecycleBuilder<TContract>(_services, alias ?? typeof(TContract).FullName ?? typeof(TContract).Name);
        }

        public override IComponentBuilder<ILifecycleComponent, object> Register(Type contractType, string? alias = null)
        {
            return Register(contractType, alias ?? contractType.FullName ?? contractType.Name);
        }
    }
}
