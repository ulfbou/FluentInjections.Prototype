// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions;

using Microsoft.Extensions.Hosting;

namespace FluentInjections.Adapters.AspNetCore
{
    public class CoreHostBuilderAdapter : IAppBuilderCore<CoreHostApplicationAdapter>
    {
        private readonly IHostBuilder _innerBuilder;

        public CoreHostBuilderAdapter(IHostBuilder builder)
        {
            _innerBuilder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        public Task<CoreHostApplicationAdapter> BuildAsync()
        {
            var host = _innerBuilder.Build();
            return Task.FromResult(new CoreHostApplicationAdapter(host, this));
        }

        // No IWebAppMiddlewareBuilderExtension implementation - HostBuilder doesn't have web middleware
        // Could potentially have other extensions later for HostBuilder-specific features (e.g., service config extensions)
    }
}
