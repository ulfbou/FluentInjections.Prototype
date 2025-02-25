// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Application;
using FluentInjections.Components;
using FluentInjections.Configurators;
using FluentInjections.DependencyInjection;
using FluentInjections.Internal;
using FluentInjections.Validation;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FluentInjections.Middlewares
{
    internal class MiddlewareConfigurator<TBuilder> : ConfiguratorBase<IMiddlewareComponent>, IMiddlewareConfigurator<TBuilder>
        where TBuilder : IApplicationBuilder<TBuilder>
    {
        public IApplication<TBuilder> Application { get; }

        public MiddlewareConfigurator(IComponentResolver<IMiddlewareComponent> internalResolver, IApplication<TBuilder> application, IServiceCollection? services = null)
            : base(internalResolver, services)
        {
            Guard.NotNull(application, nameof(application));
            Application = application;
        }

        public override async ValueTask<IComponentBuilder<IMiddlewareComponent, TContract>> RegisterAsync<TContract>(string? alias = null)
        {
            alias ??= typeof(TContract).FullName ?? typeof(TContract).Name;

            var loggerFactory = await _internalResolver.ResolveSingleAsync<ILoggerFactory>(nameof(InternalLoggerFactory));

            if (loggerFactory is null)
            {
                throw new InvalidOperationException($"No logger factory registered.");
            }

            return new FluentMiddlewareBuilder<TBuilder, TContract>(_internalResolver, Application, loggerFactory, alias);
        }

        public override async ValueTask<IComponentBuilder<IMiddlewareComponent, object>> RegisterAsync(Type contractType, string? alias = null)
        {
            Guard.NotNull(contractType, nameof(contractType));

            alias ??= contractType.FullName ?? contractType.Name;
            var loggerFactory = await _internalResolver.ResolveSingleAsync<ILoggerFactory>(nameof(InternalLoggerFactory));

            if (loggerFactory is null)
            {
                throw new InvalidOperationException($"No logger factory registered.");
            }

            return new FluentMiddlewareBuilder<TBuilder, object>(_internalResolver, Application, loggerFactory, alias);
        }
    }
}
