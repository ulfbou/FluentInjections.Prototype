// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Events;
using FluentInjections.Validation;

using Microsoft.Extensions.DependencyInjection;

namespace FluentInjections.DependencyInjection;

public class ServiceProviderWrapper : IServiceProviderWrapper, IServiceProvider
{
    private readonly IServiceProvider _innerProvider;
    private readonly Events.IConcurrentEventBus _eventBus;

    public Guid ScopeId { get; private set; }

    public ServiceProviderWrapper(IServiceProvider innerProvider, Events.IConcurrentEventBus eventBus)
    {
        _innerProvider = innerProvider;
        _eventBus = eventBus;

        if (innerProvider is IServiceProviderWrapper wrapper)
        {
            ScopeId = wrapper.ScopeId;
        }
        else
        {
            ScopeId = Guid.NewGuid();
        }
    }

    public object? GetService(Type serviceType) => _innerProvider.GetService(serviceType);
    public T? GetService<T>() => _innerProvider.GetService<T>();

    public IScopeWrapper CreateScope()
    {
        var innerScope = _innerProvider.CreateScope();
        var scopeWrapper = new InnerScopeWrapper(innerScope, _eventBus);

        _eventBus.PublishAsync(new ScopeCreatedEvent(ScopeId, scopeWrapper.Id), TimeSpan.FromSeconds(30), CancellationToken.None).GetAwaiter().GetResult();
        return scopeWrapper;
    }

    public async Task<IScopeWrapper> CreateScopeAsync(CancellationToken cancellationToken)
    {
        var innerScope = _innerProvider.CreateScope();
        var scopeWrapper = new InnerScopeWrapper(innerScope, _eventBus);
        return await _eventBus.PublishAsync(new ScopeCreatedEvent(ScopeId, scopeWrapper.Id), TimeSpan.FromSeconds(30), CancellationToken.None)
                              .ContinueWith(_ => scopeWrapper, cancellationToken);
    }
    public Task<object?> GetService(Type serviceType, CancellationToken cancellationToken)
    {
        Guard.NotNull(serviceType, nameof(serviceType));
        Guard.NotNull(cancellationToken, nameof(cancellationToken));
        return Task.FromResult(_innerProvider.GetService(serviceType));
    }
    public Task<T?> GetService<T>(CancellationToken cancellationToken)
    {
        Guard.NotNull(cancellationToken, nameof(cancellationToken));
        return Task.FromResult(_innerProvider.GetService<T>());
    }

    private class InnerScopeWrapper : IScopeWrapper
    {
        private readonly IServiceScope _innerScope;
        private readonly Events.IConcurrentEventBus _eventBus;
        private readonly object _disposeLock = new object();
        private bool _disposed;

        public InnerScopeWrapper(IServiceScope innerScope, Events.IConcurrentEventBus eventBus)
        {
            _innerScope = innerScope;
            _eventBus = eventBus;
            ServiceProvider = new ServiceProviderWrapper(_innerScope.ServiceProvider, eventBus);
            Id = Guid.NewGuid();
        }

        public IServiceProvider ServiceProvider { get; }
        public Guid Id { get; }

        public void Dispose()
        {
            lock (_disposeLock)
            {
                if (!_disposed)
                {
                    _innerScope.Dispose();
                    _eventBus.PublishAsync(new ScopeDisposedEvent(Id), TimeSpan.FromSeconds(30), CancellationToken.None).GetAwaiter().GetResult();
                    _disposed = true;
                }
            }
        }

        public async ValueTask DisposeAsync()
        {
            bool shouldDispose;

            lock (_disposeLock)
            {
                if (_disposed)
                {
                    shouldDispose = false;
                }
                else
                {
                    shouldDispose = true;
                    _disposed = true;
                }
            }

            if (shouldDispose)
            {
                if (_innerScope is IAsyncDisposable asyncDisposable)
                {
                    await asyncDisposable.DisposeAsync();
                }
                else
                {
                    _innerScope.Dispose();
                }

                await _eventBus.PublishAsync(new ScopeDisposedEvent(Id), TimeSpan.FromSeconds(30), CancellationToken.None);
            }
        }
    }
}
