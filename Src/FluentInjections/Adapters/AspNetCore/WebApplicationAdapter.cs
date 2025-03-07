// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions.Adapters;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FluentInjections.Adapters.AspNetCore
{
    public class WebApplicationAdapter : IApplicationAdapter<WebApplication>
    {
        public WebApplication Application { get; }
        private readonly ILogger<WebApplicationAdapter> _logger;

        public WebApplicationAdapter(WebApplication application, ILogger<WebApplicationAdapter> logger)
        {
            Application = application;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Starting WebApplication...");
            return Application.StartAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Stopping WebApplication...");
            return Application.StopAsync(cancellationToken);
        }

        public Task RunAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Running WebApplication...");
            return Application.RunAsync(cancellationToken);
        }

        public async ValueTask DisposeAsync()
        {
            _logger.LogInformation("Disposing WebApplicationAdapter...");
            await StopAsync();
            Application.Dispose();
        }
    }
}
