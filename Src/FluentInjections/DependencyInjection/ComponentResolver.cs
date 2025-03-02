// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Nito.AsyncEx;

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace FluentInjections.DependencyInjection
{
    public class ComponentResolver<TComponent> : IComponentResolver<TComponent>
        where TComponent : IComponent
    {
        private readonly IComponentRegistry<TComponent> _componentRegistry;
        private readonly ILogger<ComponentResolver<TComponent>> _logger;
        private readonly AsyncLock _asyncLock = new AsyncLock();
        private readonly IServiceProvider _serviceProvider;
        private readonly IServiceScope? _serviceScope;

        public ComponentResolver(IComponentRegistry<TComponent> componentRegistry, ILogger<ComponentResolver<TComponent>> logger, IServiceProvider serviceProvider, IServiceScope? serviceScope = null)
        {
            _componentRegistry = componentRegistry ?? throw new ArgumentNullException(nameof(componentRegistry));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _serviceScope = serviceScope;
        }

        public IComponentRegistry<TComponent> ComponentRegistry => _componentRegistry;

        public ValueTask DisposeAsync()
        {
            if (_serviceScope != null)
            {
                _serviceScope.Dispose();
            }

            return ValueTask.CompletedTask;
        }

        public async IAsyncEnumerable<TContract> ResolveManyAsync<TContract>(
            Func<IComponentDescriptor<TComponent, TContract>, ValueTask<bool>>? predicate = null,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            using (await _asyncLock.LockAsync(cancellationToken).ConfigureAwait(false))
            {
                await foreach (var descriptor in _componentRegistry.GetManyAsync<IComponentDescriptor<TComponent, TContract>, TContract>(cancellationToken: cancellationToken).ConfigureAwait(false))
                {
                    if (predicate == null || await predicate(descriptor).ConfigureAwait(false))
                    {
                        if (descriptor.Instance != null)
                        {
                            yield return descriptor.Instance;
                        }
                        else if (descriptor.Factory != null)
                        {
                            yield return await descriptor.Factory(this).ConfigureAwait(false);
                        }
                        else
                        {
                            yield return GetInstanceFromServiceProvider(descriptor);
                        }
                    }
                }
            }
        }

        public async IAsyncEnumerable<TContract> ResolveManyAsync<TContract>(
            IEnumerable<string> aliases,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            using (await _asyncLock.LockAsync(cancellationToken).ConfigureAwait(false))
            {
                await foreach (var descriptor in _componentRegistry.GetManyAsync<IComponentDescriptor<TComponent, TContract>, TContract>(aliases, cancellationToken).ConfigureAwait(false))
                {
                    if (descriptor.Instance != null)
                    {
                        yield return descriptor.Instance;
                    }
                    else if (descriptor.Factory != null)
                    {
                        yield return await descriptor.Factory(this).ConfigureAwait(false);
                    }
                    else
                    {
                        yield return GetInstanceFromServiceProvider(descriptor);
                    }
                }
            }
        }

        public IAsyncEnumerable<TContract> ResolveManyAsyncSimple<TContract>(Func<IComponentDescriptor<TComponent, TContract>, ValueTask<bool>>? predicate = null, CancellationToken cancellationToken = default) => throw new NotImplementedException();

        public async ValueTask<TContract?> ResolveSingleAsync<TContract>(
            Func<IComponentDescriptor<TComponent, TContract>, ValueTask<bool>>? predicate = null,
            CancellationToken cancellationToken = default)
        {
            using (await _asyncLock.LockAsync(cancellationToken).ConfigureAwait(false))
            {
                await foreach (var descriptor in _componentRegistry.GetManyAsync<IComponentDescriptor<TComponent, TContract>, TContract>(predicate, cancellationToken).ConfigureAwait(false))
                {
                    if (descriptor.Instance != null)
                    {
                        return descriptor.Instance;
                    }
                    else if (descriptor.Factory != null)
                    {
                        return await descriptor.Factory(this).ConfigureAwait(false);
                    }
                    else
                    {
                        return GetInstanceFromServiceProvider(descriptor);
                    }
                }
            }
            return default;
        }

        public async ValueTask<TContract?> ResolveSingleAsync<TContract>(
            string? alias = null,
            CancellationToken cancellationToken = default)
        {
            using (await _asyncLock.LockAsync(cancellationToken).ConfigureAwait(false))
            {
                var descriptor = await _componentRegistry.GetSingleAsync<IComponentDescriptor<TComponent, TContract>, TContract>(alias, cancellationToken).ConfigureAwait(false);
                if (descriptor != null)
                {
                    if (descriptor.Instance != null)
                    {
                        return descriptor.Instance;
                    }
                    else if (descriptor.Factory != null)
                    {
                        return await descriptor.Factory(this).ConfigureAwait(false);
                    }
                    else
                    {
                        return GetInstanceFromServiceProvider(descriptor);
                    }
                }

                return default;
            }
        }

        private TContract GetInstanceFromServiceProvider<TContract>(IComponentDescriptor<TComponent, TContract> descriptor)
        {
            IServiceProvider provider = _serviceScope?.ServiceProvider ?? _serviceProvider;
            return (TContract)provider.GetRequiredService(descriptor.ContractType);
        }
    }
}
