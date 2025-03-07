// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.DependencyInjection;
using FluentInjections.Abstractions;

namespace FluentInjections.Extensions.Internal
{
    internal static class ServiceLifetimeExtensions
    {
        public static ComponentLifetime ToComponentLifetime(this ServiceLifetime lifetime)
        {
            return lifetime switch
            {
                ServiceLifetime.Scoped => ComponentLifetime.Scoped,
                ServiceLifetime.Transient => ComponentLifetime.Transient,
                ServiceLifetime.Singleton => ComponentLifetime.Singleton,
                _ => throw new InvalidOperationException("Invalid service lifetime")
            };
        }
    }
}
