// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions;
using FluentInjections.DependencyInjection;

namespace FluentInjections.Internal.Core
{
    public class ResolverServiceProvider<TComponent> : IServiceProvider where TComponent : IComponent
    {
        private readonly IComponentResolver<TComponent> _resolver;

        public ResolverServiceProvider(IComponentResolver<TComponent> resolver)
        {
            _resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
        }

        public object? GetService(Type serviceType)
        {
            return _resolver.ResolveSingleAsync(serviceType, cancellationToken: CancellationToken.None).GetAwaiter().GetResult();
        }
    }
}
