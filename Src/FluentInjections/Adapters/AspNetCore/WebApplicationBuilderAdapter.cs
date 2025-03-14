// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions.Adapters;
using FluentInjections.Core.Discovery.Metadata;
using FluentInjections.Validation;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FluentInjections.Adapters.AspNetCore
{
    [AdapterMetadataAttribute(typeof(WebApplication), frameworkVersion: "9.0")]
    public class WebApplicationBuilderAdapter : IApplicationBuilderAdapter<WebApplication>
    {
        private readonly WebApplicationBuilder _builder;

        public WebApplicationBuilderAdapter()
        {
            _builder = WebApplication.CreateBuilder();
            _builder.Logging.ClearProviders()
                            .AddConsole()
                            .AddDebug();
            _builder.Services.AddLogging(builder => builder.ClearProviders()
                                                           .AddConsole()
                                                           .AddDebug());
        }

        public object InnerBuilder => _builder;

        public Task<IApplicationAdapter<WebApplication>> BuildAsync(CancellationToken cancellationToken = default)
        {
            var application = _builder.Build();
            var loggerFactory = application.Services.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger<WebApplicationAdapter>();
            return Task.FromResult<IApplicationAdapter<WebApplication>>(new WebApplicationAdapter(application, logger));
        }

        public void ConfigureServices(IServiceCollection services)
        {
            Guard.NotNull(services, nameof(services));

            foreach (var service in services)
            {
                _builder.Services.Add(service);
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (InnerBuilder is IAsyncDisposable asyncDisposable)
            {
                await asyncDisposable.DisposeAsync();
            }
            else if (InnerBuilder is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}
