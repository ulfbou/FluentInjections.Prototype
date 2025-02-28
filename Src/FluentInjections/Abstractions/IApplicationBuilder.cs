// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions.Adapters;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FluentInjections.Abstractions
{
    public interface IApplicationBuilder<TConcreteBuilder, TConcreteApplication, TConcreteBuilderAdapter, TConcreteApplicationAdapter>
            where TConcreteBuilder : notnull
            where TConcreteApplication : notnull
            where TConcreteBuilderAdapter : notnull, IConcreteBuilderAdapter<TConcreteBuilder, TConcreteApplication>
            where TConcreteApplicationAdapter : notnull, IConcreteApplicationAdapter<TConcreteApplication>
    {
        TConcreteBuilderAdapter InnerAdapter { get; }
        IConfiguration Configuration { get; }
        ILoggerFactory LoggerFactory { get; }
        Task<IApplicationAdapter<TConcreteApplication, TConcreteApplicationAdapter>> BuildAsync(CancellationToken cancellationToken);
    }
}
