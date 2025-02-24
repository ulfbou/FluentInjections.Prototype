// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.DependencyInjection;

namespace FluentInjections.DependencyInjection;

public class ServiceProviderWrapper : IServiceProviderWrapper
{
    private readonly IServiceProvider _serviceProvider;

    public Guid ScopeId { get; } = Guid.NewGuid();

    public ServiceProviderWrapper(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<IScopeWrapper> CreateScopeAsync(CancellationToken cancellationToken)
    {
        var scope = _serviceProvider.CreateScope();
        return await Task.FromResult(new InnerScopeWrapper(scope.ServiceProvider));
    }

    public async Task<object?> GetService(Type serviceType, CancellationToken cancellationToken)
    {
        return await Task.FromResult(_serviceProvider.GetService(serviceType));
    }

    public async Task<T?> GetService<T>(CancellationToken cancellationToken)
    {
        try
        {
            return await Task.FromResult((T?)_serviceProvider.GetService(typeof(T)));
        }
        catch
        {
            return default;
        }
    }

    private class InnerScopeWrapper : IScopeWrapper
    {
        public Guid Id { get; } = Guid.NewGuid();
        public IServiceProvider ServiceProvider { get; }

        public InnerScopeWrapper(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public ValueTask DisposeAsync()
        {
            Dispose();
            return ValueTask.CompletedTask;
        }

        public void Dispose()
        {
            if (ServiceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}
