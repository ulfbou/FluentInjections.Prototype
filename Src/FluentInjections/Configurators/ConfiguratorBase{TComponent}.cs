// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Components;
using FluentInjections.DependencyInjection;
using FluentInjections.Validation;

using Microsoft.Extensions.DependencyInjection;

namespace FluentInjections.Configurators
{
    public abstract class ConfiguratorBase<TComponent> : ConfiguratorBase, IConfigurator<TComponent>
        where TComponent : IComponent
    {
        protected readonly IComponentResolver<TComponent> _internalResolver;
        protected readonly IServiceCollection _services;

        protected ConfiguratorBase(IComponentResolver<TComponent> internalResolver, IServiceCollection? services = null)
        {
            Guard.NotNull(internalResolver, nameof(internalResolver));
            _internalResolver = internalResolver;
            _services = services ?? new ServiceCollection();
        }

        public abstract ValueTask<IComponentBuilder<TComponent, TContract>> RegisterAsync<TContract>(string? alias = null);
        public abstract ValueTask<IComponentBuilder<TComponent, object>> RegisterAsync(Type contractType, string? alias = null);
    }
}
