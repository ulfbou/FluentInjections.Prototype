// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.DependencyInjection;

namespace FluentInjections;

public interface IServiceProviderFactory
{
    IServiceProvider CreateServiceProvider(IServiceCollection services);
    Task<IServiceProvider> CreateServiceProviderAsync(IServiceCollection services, CancellationToken cancellationToken);
}
