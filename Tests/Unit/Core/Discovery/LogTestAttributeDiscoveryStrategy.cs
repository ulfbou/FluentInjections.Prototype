// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Core.Abstractions;

using Microsoft.Extensions.Logging;
using FluentInjections.Core.Discovery;
using FluentInjections.Tests.Utils.Logging;
using FluentInjections.Validation;

namespace FluentInjections.Tests.Units.Core.Discovery
{
    internal class LogTestAttributeDiscoveryStrategy : ITypeDiscoveryStrategy
    {
        private readonly ILoggerFactory _factory;

        public LogTestAttributeDiscoveryStrategy(ILoggerFactory factory)
        {
            Guard.NotNull(factory, nameof(factory));
            _factory = factory;
        }

        public IAsyncEnumerable<Type> Discover(ITypeDiscoveryContext context, CancellationToken cancellationToken = default)
        {
            Guard.NotNull(context, nameof(context));
            var logger = _factory.CreateLogger<AttributeDiscoveryStrategy>();
            logger.LogDebug("Discovering types...");
            return AsyncEnumerable.Empty<Type>();
        }
    }
}
