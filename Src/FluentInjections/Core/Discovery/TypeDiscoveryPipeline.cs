// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Core.Abstractions;
using FluentInjections.Validation;

using Microsoft.Extensions.Logging;

using System.Runtime.CompilerServices;

namespace FluentInjections.Core.Discovery
{
    public class TypeDiscoveryPipeline
    {
        private readonly IEnumerable<ITypeDiscoveryStrategy> _strategies;
        private readonly ILoggerFactory _loggerFactory;

        public TypeDiscoveryPipeline(IEnumerable<ITypeDiscoveryStrategy> strategies, ILoggerFactory loggerFactory)
        {
            Guard.NotNull(strategies, nameof(strategies));
            Guard.NotNull(loggerFactory, nameof(loggerFactory));
            _strategies = strategies;
            _loggerFactory = loggerFactory;
            _loggerFactory.CreateLogger<TypeDiscoveryPipeline>().LogDebug("TypeDiscoveryPipeline created with {Count} strategies", _strategies.Count());
        }

        public async IAsyncEnumerable<Type> DiscoverTypes(TypeDiscoveryContext context, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            foreach (var strategy in _strategies)
            {
                await foreach (var type in strategy.Discover(context, cancellationToken))
                {
                    yield return type;
                }
            }
        }
    }
}
