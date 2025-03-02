// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions;

using Microsoft.Extensions.DependencyInjection;

namespace FluentInjections.DependencyInjection
{
    public static class ComponentLifetimeExtensions
    {
        public static ServiceLifetime ToServiceLifetime(this ComponentLifetime lifetime)
        {
            return lifetime switch
            {
                ComponentLifetime.Transient => ServiceLifetime.Transient,
                ComponentLifetime.Scoped => ServiceLifetime.Scoped,
                ComponentLifetime.Singleton => ServiceLifetime.Singleton,
                _ => throw new ArgumentOutOfRangeException(nameof(lifetime), lifetime, null)
            };
        }
    }
}
