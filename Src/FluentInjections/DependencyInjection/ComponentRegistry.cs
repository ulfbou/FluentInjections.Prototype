// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections;
using FluentInjections.Descriptors;
using FluentInjections.Validation;

using Microsoft.Extensions.Logging;

using Nito.AsyncEx;

using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace FluentInjections.DependencyInjection;

/// <inheritdoc/>
public class ComponentRegistry<TComponent> : IComponentRegistry<TComponent> where TComponent : IComponent
{
    private readonly ConcurrentDictionary<string, IComponentDescriptor<TComponent>> _descriptors = new(); // Changed to IComponentDescriptor<TComponent>
    private readonly AsyncLock _asyncLock = new();
    private readonly ILogger<ComponentRegistry<TComponent>> _logger;
    private readonly TimeSpan _operationTimeout;

    public ComponentRegistry(ILogger<ComponentRegistry<TComponent>> logger, TimeSpan operationTimeout)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _operationTimeout = operationTimeout;
    }

    public async ValueTask RegisterAsync<TDescriptor>(TDescriptor descriptor, CancellationToken cancellationToken = default)
        where TDescriptor : IComponentDescriptor<TComponent>
    {
        Guard.NotNull(descriptor, nameof(descriptor));

        _logger.LogInformation("Registering descriptor with alias {Alias}.", descriptor.Alias);
        Debug.WriteLine("Registering descriptor with alias {Alias}.", descriptor.Alias);

        using (var timeoutCts = new CancellationTokenSource(_operationTimeout))
        using (var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutCts.Token))
        {
            try
            {
                using (await _asyncLock.LockAsync(linkedCts.Token))
                {
                    _descriptors[descriptor.Alias] = descriptor;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while attempting to register a descriptor with alias '{Alias}'.", descriptor.Alias);
                Debug.WriteLine("{Exception} An error occurred while attempting to register a descriptor with alias '{Alias}'.", ex.Message, descriptor.Alias);
            }
        }
    }

    /// <inheritdoc/>
    public async ValueTask<bool> UnregisterAsync(string? alias = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(alias))
        {
            return false;
        }

        _logger.LogInformation("Unregistering descriptor with alias {Alias}.", alias);
        Debug.WriteLine("Unregistering descriptor with alias {Alias}.", alias);

        using (var timeoutCts = new CancellationTokenSource(_operationTimeout))
        using (var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutCts.Token))
        {
            try
            {
                using (await _asyncLock.LockAsync(linkedCts.Token))
                {
                    return _descriptors.TryRemove(alias, out _);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while attempting to unregister a descriptor with alias '{Alias}'.", alias);
                Debug.WriteLine("{Exception} An error occurred while attempting to unregister a descriptor with alias '{Alias}'.", ex, alias);
                return false;
            }
        }
    }

    /// <inheritdoc/>
    public async IAsyncEnumerable<TDescriptor> GetManyAsync<TDescriptor, TContract>(
        IEnumerable<string> aliases,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
        where TDescriptor : IComponentDescriptor<TComponent, TContract>
    {
        _logger.LogInformation("Getting multiple descriptors by aliases.");

        var aliasSet = aliases.ToHashSet();

        await foreach (var descriptor in GetManyAsyncInternal<TDescriptor, TContract>(d => ValueTask.FromResult(aliasSet.Contains(d.Alias)), cancellationToken))
        {
            yield return descriptor;
        }
    }

    /// <inheritdoc/>
    public async IAsyncEnumerable<TDescriptor> GetManyAsync<TDescriptor, TContract>(
        Func<TDescriptor, ValueTask<bool>>? predicate = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
        where TDescriptor : IComponentDescriptor<TComponent, TContract>
    {
        _logger.LogInformation("Getting multiple descriptors by predicate.");
        Debug.WriteLine("Getting multiple descriptors by predicate.");

        await foreach (var descriptor in GetManyAsyncInternal<TDescriptor, TContract>(predicate, cancellationToken))
        {
            yield return descriptor;
        }
    }

    /// <inheritdoc/>
    public async ValueTask<TDescriptor?> GetSingleAsync<TDescriptor, TContract>(string? alias = null, CancellationToken cancellationToken = default)
        where TDescriptor : IComponentDescriptor<TComponent, TContract>
    {
        if (string.IsNullOrWhiteSpace(alias))
        {
            return default;
        }

        _logger.LogInformation("Getting single descriptor by alias {Alias}.", alias);
        Debug.WriteLine("Getting single descriptor by alias {Alias}.", alias);

        using (var timeoutCts = new CancellationTokenSource(_operationTimeout))
        using (var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutCts.Token))
        {
            try
            {
                using (await _asyncLock.LockAsync(linkedCts.Token))
                {
                    if (_descriptors.TryGetValue(alias, out var descriptor))
                    {
                        return (TDescriptor)descriptor;
                    }
                    return default;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while attempting to get a descriptor with alias '{Alias}'.", alias);
                Debug.WriteLine("{Exception} An error occurred while attempting to get a descriptor with alias '{Alias}'.", ex, alias);
                return default;
            }
        }
    }

    /// <inheritdoc/>
    public async ValueTask<TDescriptor?> GetSingleAsync<TDescriptor, TContract>(
        Func<TDescriptor, ValueTask<bool>> predicate,
        CancellationToken cancellationToken = default)
        where TDescriptor : IComponentDescriptor<TComponent, TContract>
    {
        if (predicate == null)
        {
            throw new ArgumentNullException(nameof(predicate));
        }

        _logger.LogInformation("Getting single descriptor by predicate.");
        Debug.WriteLine("Getting single descriptor by predicate.");

        using (var timeoutCts = new CancellationTokenSource(_operationTimeout))
        using (var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutCts.Token))
        {
            try
            {
                using (await _asyncLock.LockAsync(linkedCts.Token))
                {
                    foreach (var kvp in _descriptors)
                    {
                        var descriptor = (TDescriptor)kvp.Value;

                        if (await predicate(descriptor))
                        {
                            return descriptor;
                        }

                        if (cancellationToken.IsCancellationRequested)
                        {
                            return default;
                        }
                    }
                    return default;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while attempting to get a descriptor.");
                Debug.WriteLine("{Exception} An error occurred while attempting to get a descriptor.");
                return default;
            }
        }
    }

    /// <summary>
    /// Gets multiple component descriptors asynchronously by aliases.
    /// </summary>
    /// <typeparam name="TDescriptor">The type of the component descriptor.</typeparam>
    /// <typeparam name="TContract">The type of the contract.</typeparam>
    /// <param name="predicate">The predicate to match the component descriptors.</param>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>An asynchronous enumerable of component descriptors.</returns>
    private async IAsyncEnumerable<TDescriptor> GetManyAsyncInternal<TDescriptor, TContract>(
        Func<TDescriptor, ValueTask<bool>>? predicate,
        [EnumeratorCancellation] CancellationToken cancellationToken)
        where TDescriptor : IComponentDescriptor<TComponent, TContract>
    {
        using (var timeoutCts = new CancellationTokenSource(_operationTimeout))
        using (var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutCts.Token))
        {
            using (await _asyncLock.LockAsync(linkedCts.Token))
            {
                foreach (var kvp in _descriptors)
                {
                    TDescriptor? descriptor = default;

                    try
                    {
                        descriptor = (TDescriptor)kvp.Value;

                        if (kvp.Value is not TDescriptor || (predicate is not null && !await predicate(descriptor)))
                        {
                            continue;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "An error occurred while attempting to get a descriptor with alias '{Alias}'.", kvp.Key);
                        Debug.WriteLine("{Exception} An error occurred while attempting to get a descriptor with alias '{Alias}'.", ex.Message, kvp.Key);
                    }

                    if (descriptor is not null)
                    {
                        yield return descriptor;
                    }

                    if (linkedCts.Token.IsCancellationRequested)
                    {
                        yield break;
                    }
                }
            }
        }
    }
}
