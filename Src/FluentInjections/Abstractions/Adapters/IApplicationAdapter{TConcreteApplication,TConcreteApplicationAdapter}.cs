// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.Logging;

namespace FluentInjections.Abstractions.Adapters
{
    public interface IApplicationAdapter<TConcreteApplication, TConcreteApplicationAdapter>
            where TConcreteApplication : notnull
            where TConcreteApplicationAdapter : notnull, IConcreteApplicationAdapter<TConcreteApplication>
    {
        TConcreteApplicationAdapter Adapter { get; }
        ILoggerFactory LoggerFactory { get; }
        Task RunAsync(CancellationToken cancellationToken = default);
    }
}
