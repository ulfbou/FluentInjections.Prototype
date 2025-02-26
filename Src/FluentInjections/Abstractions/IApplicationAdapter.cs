// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Adapters;
using FluentInjections.Logging;

namespace FluentInjections.Abstractions
{
    public interface IMiddlewareCapableApplicationAdapter : IApplicationAdapter // Extends IApplicationAdapter
    {
        void RegisterMiddleware(Func<ApplicationDelegate, ApplicationDelegate> middleware); // Method for adapter to register middleware
    }
    public interface IApplicationAdapter
    {
        Task RunAsync(CancellationToken? cancellationToken = null);
        Task StopAsync(CancellationToken? cancellationToken = null);
        ILoggerFactoryProvider? LoggerFactoryProvider { get; }
    }
}
