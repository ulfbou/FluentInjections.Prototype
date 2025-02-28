// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Http;

namespace FluentInjections.Tests.Utils
{
    public interface IWebApplication
    {
        IServiceProvider Services { get; }

        Task RunAsync(CancellationToken cancellationToken = default);
        Task StartAsync(CancellationToken cancellationToken = default);
        Task StopAsync(CancellationToken cancellationToken = default);
        void Use(Func<RequestDelegate, RequestDelegate> middleware);
    }
}
