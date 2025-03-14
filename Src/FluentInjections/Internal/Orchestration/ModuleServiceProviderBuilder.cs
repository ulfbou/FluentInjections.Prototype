// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions;
using FluentInjections.Extensions;
using FluentInjections.Internal.Discovery;
using FluentInjections.Metadata;
using FluentInjections.Validation;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using System.Reflection;

namespace FluentInjections.Internal.Orchestration
{
    internal class ModuleServiceProviderBuilder
    {
        private readonly IModuleDiscoveryService _moduleDiscoveryService;
        private readonly IConfiguration _configuration;
        private readonly ILoggerFactory _loggerFactory;
        private readonly IEnumerable<Assembly>? _assemblies;
        private IAsyncEnumerable<ModuleMetadata>? _metadata;

        public ModuleServiceProviderBuilder(
            IModuleDiscoveryService discoveryService,
            IConfiguration configuration,
            ILoggerFactory loggerFactory,
            IEnumerable<Assembly>? assemblies = null)
        {
            Guard.NotNull(discoveryService, nameof(discoveryService));
            Guard.NotNull(configuration, nameof(configuration));
            Guard.NotNull(loggerFactory, nameof(loggerFactory));
            _moduleDiscoveryService = discoveryService;
            _configuration = configuration;
            _loggerFactory = loggerFactory;
            _assemblies = assemblies;
            _metadata = null;
        }

        internal Task<IServiceProvider> BuildServiceProviderAsync<TComponent>(CancellationToken cancellationToken = default)
            where TComponent : IComponent
        {
            if (_metadata is null)
            {
                lock (_moduleDiscoveryService)
                {
                    if (_metadata is null)
                    {
                        //_metadata = _moduleDiscoveryService.DiscoverModulesAsync(_assemblies ?? Enumerable.Empty<Assembly>(), cancellationToken);
                    }
                }
            }

            var services = new ServiceCollection();

            //await _moduleDiscoveryService.DiscoverModulesAsync(_assemblies ?? Enumerable.Empty<Assembly>(), cancellationToken);

            return Task.FromResult<IServiceProvider>(services.BuildServiceProvider());
        }
    }
}
