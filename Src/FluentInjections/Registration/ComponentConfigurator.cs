// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions;
using FluentInjections.Registration;
using FluentInjections.Validation;

using Microsoft.Extensions.Logging;

using System.Collections.Concurrent;

namespace FluentInjections.Internal.Registration
{
    internal abstract class ComponentConfigurator<TComponent>
        where TComponent : IComponent
    {
        protected readonly ILoggerFactory _loggerFactory;
        protected readonly ILogger _logger;
        protected readonly ConcurrentBag<IComponentRegistration<TComponent>> _registrations = new();

        public ComponentConfigurator(ILoggerFactory loggerFactory)
        {
            Guard.NotNull(loggerFactory, nameof(loggerFactory));
            _loggerFactory = loggerFactory;
            _logger = loggerFactory.CreateLogger(GetType().Name);
        }

        protected virtual ValueTask<TBuilder> RegisterInternalAsync<TContract, TBuilder>(string? alias = null, CancellationToken? cancellationToken = null)
            where TBuilder : IComponentBuilder<TComponent, TContract>
        {
            alias ??= typeof(TContract).FullName ?? typeof(TContract).Name;
            cancellationToken ??= CancellationToken.None;

            var registration = new ServiceRegistration<TContract>
            {
                Alias = alias
            };

            return CreateBuilderAsync<TContract, TBuilder>(registration, cancellationToken);
        }

        protected abstract ValueTask<TBuilder> CreateBuilderAsync<TContract, TBuilder>(ServiceRegistration<TContract> registration, CancellationToken? cancellationToken)
            where TBuilder : IComponentBuilder<TComponent, TContract>;
    }
}
