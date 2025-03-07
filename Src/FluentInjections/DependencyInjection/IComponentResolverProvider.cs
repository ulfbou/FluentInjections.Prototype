// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions;
using FluentInjections.Internal.Core;
using FluentInjections.Validation;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using System.Collections.Concurrent;

namespace FluentInjections.DependencyInjection
{
    /// <summary>
    /// Represents a provider for component resolvers.
    /// </summary>
    public interface IComponentResolverProvider
    {
        /// <summary>
        /// Retrieves a component resolver for the specified component type.
        /// </summary>
        /// <typeparam name="TComponent">The type of the component.</typeparam>
        /// <returns>The component resolver.</returns>
        ValueTask<IComponentResolver<TComponent>> GetResolverAsync<TComponent>(CancellationToken cancellationToken = default)
            where TComponent : IComponent;
    }
    public class ComponentResolverProvider : IComponentResolverProvider
    {
        private readonly IDictionary<Type, object> _registries = new ConcurrentDictionary<Type, object>();
        private readonly IDictionary<Type, object> _resolvers = new ConcurrentDictionary<Type, object>();
        private readonly IServiceProvider _provider;
        private readonly ILoggerFactory _loggerFactory;

        public ComponentResolverProvider(IServiceProvider provider, ILoggerFactory loggerFactory)
        {
            Guard.NotNull(provider, nameof(provider));
            Guard.NotNull(loggerFactory, nameof(loggerFactory));
            _provider = provider;
            _loggerFactory = loggerFactory;
        }

        public ValueTask<IComponentResolver<TComponent>> GetResolverAsync<TComponent>(CancellationToken cancellationToken = default)
            where TComponent : IComponent
        {
            return ValueTask.FromResult<IComponentResolver<TComponent>>(_provider.GetRequiredService<IComponentResolver<TComponent>>());
        }

        private ValueTask<IComponentResolver<TComponent>> GetOldResolverAsync<TComponent>(CancellationToken cancellationToken = default)
            where TComponent : IComponent
        {
            var resolver = _provider.GetService<IComponentResolver<TComponent>>()!;

            if (resolver is null)
            {
                throw new InvalidOperationException($"No component resolver was found for component type '{typeof(TComponent).FullName}'.");
            }

            _resolvers[typeof(TComponent)] = resolver;

            return ValueTask.FromResult<IComponentResolver<TComponent>>(resolver);
        }
    }
}
