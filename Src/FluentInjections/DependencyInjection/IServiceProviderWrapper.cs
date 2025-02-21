// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections.DependencyInjection;

public interface IServiceProviderWrapper
{
    Guid ScopeId { get; }

    Task<IScopeWrapper> CreateScopeAsync(CancellationToken cancellationToken);
    Task<object?> GetService(Type serviceType, CancellationToken cancellationToken);
    Task<T?> GetService<T>(CancellationToken cancellationToken);
}
