// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions.Adapters;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FluentInjections.Adapters.AspNetCore
{
    public class WebApplicationBuilderAdapter : IApplicationBuilderAdapter<WebApplication>
    {
        private readonly WebApplicationBuilder _builder;

        public WebApplicationBuilderAdapter(WebApplicationBuilder builder)
        {
            _builder = builder;
        }

        public async Task<IApplicationAdapter<WebApplication>> BuildAsync(CancellationToken cancellationToken = default)
        {
            var application = _builder.Build();
            var loggerFactory = application.Services.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger<WebApplicationAdapter>();
            return new WebApplicationAdapter(application, logger);
        }

        public ValueTask DisposeAsync()
        {
            return ValueTask.CompletedTask;
        }
    }
}
