// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Core.Discovery;

namespace FluentInjections.Core.Abstractions
{
    public interface ITypeDiscoveryStrategy
    {
        IAsyncEnumerable<Type> Discover(ITypeDiscoveryContext context, CancellationToken cancellationToken = default);
    }
}
