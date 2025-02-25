// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Application;
using FluentInjections.Builders;
using FluentInjections.Components;
using FluentInjections.DependencyInjection;
using FluentInjections.Middlewares;
using FluentInjections.Validation;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FluentInjections.Lifecycle
{
    internal class FluentLifecycleBuilder<TBuilder, TContract>
        : ComponentBuilderBase<ILifecycleComponent, TContract, ILifecycleRegistration<TContract>>
        , IComponentBuilder<ILifecycleComponent, TContract>
        where TBuilder : IApplicationBuilder<TBuilder>
    {
        private IComponentResolver<ILifecycleComponent> _internalResolver;
        private ILoggerFactory _loggerFactory;
        private string _alias;
        private readonly ILifecycleRegistration<TContract> _registration;

        public override ILifecycleRegistration<TContract> Registration => _registration;

        public FluentLifecycleBuilder(IComponentResolver<ILifecycleComponent> internalResolver, TBuilder builder, ILoggerFactory loggerFactory, string alias)
            : base(internalResolver, builder.Services, loggerFactory)
        {
            Guard.NotNull(internalResolver, nameof(internalResolver));
            Guard.NotNull(loggerFactory, nameof(loggerFactory));
            Guard.NotNullOrWhiteSpace(alias, nameof(alias));
            _internalResolver = internalResolver;
            _loggerFactory = loggerFactory;
            _alias = alias;
            _registration = new LifecycleRegistration<TContract> { Alias = alias };
        }
    }
}
