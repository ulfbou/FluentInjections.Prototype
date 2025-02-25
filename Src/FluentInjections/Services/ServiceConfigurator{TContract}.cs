// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Application;
using FluentInjections.Components;
using FluentInjections.Configurators;
using FluentInjections.DependencyInjection;
using FluentInjections.Internal;
using FluentInjections.Middlewares;
using FluentInjections.Validation;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FluentInjections.Services
{
    public class ServiceConfigurator<TBuilder> : ConfiguratorBase<IServiceComponent>, IServiceConfigurator, IConfigurator<IServiceComponent>
        where TBuilder : class, IApplicationBuilder<TBuilder>
    {
        private readonly TBuilder _builder;

        public ServiceConfigurator(IComponentResolver<IServiceComponent> internalResolver, TBuilder builder, IServiceCollection? services = null)
            : base(internalResolver, services)
        {
            Guard.NotNull(builder, nameof(builder));
            _builder = builder;
        }

        public IServiceCollection Services => _services;

        public override async ValueTask<IComponentBuilder<IServiceComponent, TContract>> RegisterAsync<TContract>(string? alias = null)
        {
            alias ??= typeof(TContract).FullName ?? typeof(TContract).Name;

            var loggerFactory = await _internalResolver.ResolveSingleAsync<ILoggerFactory>(nameof(InternalLoggerFactory));

            if (loggerFactory is null)
            {
                throw new InvalidOperationException($"No logger factory registered.");
            }

            return new ServiceBuilder<TBuilder, TContract>(_internalResolver, _builder, loggerFactory, alias);
        }

        public override async ValueTask<IComponentBuilder<IServiceComponent, object>> RegisterAsync(Type contractType, string? alias = null)
        {
            Guard.NotNull(contractType, nameof(contractType));

            alias ??= contractType.FullName ?? contractType.Name;
            var loggerFactory = await _internalResolver.ResolveSingleAsync<ILoggerFactory>(nameof(InternalLoggerFactory));

            if (loggerFactory is null)
            {
                throw new InvalidOperationException($"No logger factory registered.");
            }

            return new ServiceBuilder<TBuilder, object>(_internalResolver, _builder, loggerFactory, alias);
        }
    }
}
