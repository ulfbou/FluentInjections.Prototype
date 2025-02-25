// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FluentInjections.Application
{
    public class FluentApplication<TBuilder> : IApplication<TBuilder>
        where TBuilder : IApplicationBuilder<TBuilder>
    {
        public TBuilder Builder { get; }
        public IHost Host { get; }
        public IServiceCollection Services => Builder.Services;
        public IServiceProvider Provider => Host.Services;

        public FluentApplication(TBuilder builder, IHost host)
        {
            Builder = builder;
            Host = host;
        }

        public async Task RunAsync(CancellationToken cancellationToken = default)
        {
            await Host.RunAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken = default)
        {
            await Host.StopAsync(cancellationToken);
        }

        public async Task StartAsync(CancellationToken cancellationToken = default)
        {
            await Host.StartAsync(cancellationToken);
        }

        public Task<TInnerApplication> GetInnerApplicationAsync<TInnerApplication>(CancellationToken cancellationToken = default)
        {
            if (Host is TInnerApplication innerApplication)
            {
                return Task.FromResult(innerApplication);
            }

            throw new InvalidOperationException($"The underlying host is not of type {typeof(TInnerApplication).FullName}");
        }
    }
}
