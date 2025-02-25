// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Application;
using FluentInjections.Builders;
using FluentInjections.Components;
using FluentInjections.DependencyInjection;
using FluentInjections.Validation;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FluentInjections.Services
{
    internal sealed class ServiceBuilder<TBuilder, TContract>
        : ComponentBuilderBase<IServiceComponent, TContract, IServiceRegistration<TContract>>
        , IServiceBuilder<TContract>
        , IComponentBuilder<IServiceComponent, TContract>
        where TBuilder : IApplicationBuilder<TBuilder>
    {
        private readonly TBuilder _builder;
        private readonly IServiceRegistration<TContract> _registration;

        public ServiceBuilder(IComponentResolver<IServiceComponent> innerResolver, TBuilder builder, ILoggerFactory loggerFactory, string alias)
            : base(innerResolver, builder.Services, loggerFactory)
        {
            Guard.NotNull(builder, nameof(builder));
            Guard.NotNullOrWhiteSpace(alias, nameof(alias));
            _builder = builder;
            _registration = new ServiceRegistration<TContract>
            {
                Alias = alias
            };
        }

        public override IServiceRegistration<TContract> Registration => _registration;
    }
}
