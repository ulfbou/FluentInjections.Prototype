// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System.Threading;
using System.Threading.Tasks;

namespace FluentInjections.Application
{
    public interface IApplication<TBuilder>
        where TBuilder : IApplicationBuilder<TBuilder>
    {
        TBuilder Builder { get; }
        IHost Host { get; }
        Task RunAsync(CancellationToken cancellationToken = default);
        Task StopAsync(CancellationToken cancellationToken = default);
        Task StartAsync(CancellationToken cancellationToken = default);
        Task<TInnerApplication> GetInnerApplicationAsync<TInnerApplication>(CancellationToken cancellationToken = default);
    }
}
