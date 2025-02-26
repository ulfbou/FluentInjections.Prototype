// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.Logging;

namespace FluentInjections.Abstractions
{
    public interface IApplication<TConcreteApplication, TConcreteApplicationAdapter>
        where TConcreteApplicationAdapter : IConcreteApplicationAdapter<TConcreteApplication>
    {
        TConcreteApplicationAdapter Adapter { get; }
        ILoggerFactory LoggerFactory => Adapter.LoggerFactoryProvider?.GetLoggerFactory() ?? Logging.NullLoggerFactory.Instance;
        Task RunAsync(CancellationToken? cancellationToken = null);
        Task StopAsync(CancellationToken? cancellationToken = null);
    }
}
