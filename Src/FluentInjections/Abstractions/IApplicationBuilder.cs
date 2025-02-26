// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FluentInjections.Abstractions
{
    public interface IApplicationBuilder<TConcreteBuilder, TConcreteApplication, TConcreteBuilderAdapter, TConcreteApplicationAdapter>
        where TConcreteBuilderAdapter : IConcreteBuilderAdapter<TConcreteBuilder, TConcreteApplication>
        where TConcreteApplicationAdapter : IConcreteApplicationAdapter<TConcreteApplication>
    {
        TConcreteBuilderAdapter InnerAdapter { get; }
        IConfiguration Configuration { get; }
        ILoggerFactory LoggerFactory { get; }
        Task<IApplication<TConcreteApplication, TConcreteApplicationAdapter>> BuildAsync();
    }
}
