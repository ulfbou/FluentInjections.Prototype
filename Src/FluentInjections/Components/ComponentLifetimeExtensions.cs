// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.DependencyInjection;

namespace FluentInjections.Components
{
    public static class ComponentLifetimeExtensions
    {
        public static ServiceLifetime ToServiceLifetime(this ComponentLifetime lifetime)
        {
            return lifetime switch
            {
                ComponentLifetime.Singleton => ServiceLifetime.Singleton,
                ComponentLifetime.Scoped => ServiceLifetime.Scoped,
                ComponentLifetime.Transient => ServiceLifetime.Transient,
                _ => throw new ArgumentOutOfRangeException(nameof(lifetime), lifetime, null)
            };
        }
    }
}
