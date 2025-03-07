// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.DependencyInjection;
using FluentInjections.Abstractions;

namespace FluentInjections.Extensions.Internal
{
    internal static class ComponentLifetimeExtensions
    {
        public static ServiceLifetime ToServiceLifetime(this ComponentLifetime lifetime)
        {
            return lifetime switch
            {
                ComponentLifetime.Singleton => ServiceLifetime.Singleton,
                ComponentLifetime.Transient => ServiceLifetime.Transient,
                ComponentLifetime.Scoped => ServiceLifetime.Scoped,
                _ => throw new InvalidOperationException("Invalid component lifetime")
            };
        }
    }
}
