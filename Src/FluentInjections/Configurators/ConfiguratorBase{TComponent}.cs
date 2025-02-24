// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Components;
using FluentInjections.Validation;

using Microsoft.Extensions.DependencyInjection;

namespace FluentInjections.Configurators
{
    public abstract class ConfiguratorBase<TComponent> : ConfiguratorBase, IConfigurator<TComponent>
        where TComponent : IComponent
    {
        protected readonly IServiceCollection _services;

        protected ConfiguratorBase(IServiceCollection services)
        {
            Guard.NotNull(services, nameof(services));
            _services = services;
        }

        public virtual IComponentBuilder<TComponent, TContract> Register<TContract>(string alias)
        {
            Guard.NotNull(alias, nameof(alias));
            return new FluentComponentBuilder<TComponent, TContract>(_services, alias);
        }

        public virtual IComponentBuilder<TComponent, object> Register(Type contractType, string alias)
        {
            Guard.NotNull(contractType, nameof(contractType));
            Guard.NotNull(alias, nameof(alias));
            return new FluentComponentBuilder<TComponent, object>(_services, alias);
        }
    }
}

