// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.DependencyInjection;

namespace FluentInjections.Extensions;

public static class ServiceCollectionExtensions
{
    // Populate the service collection with the services from the provided container.
    public static Task<IServiceCollection> PopulateAsync(this IServiceCollection services, IServiceCollection container)
    {
        foreach (var service in container)
        {
            services.Add(service);
        }
        return Task.FromResult(services);
    }
}
