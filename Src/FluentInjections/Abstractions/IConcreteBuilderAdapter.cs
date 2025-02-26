// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Adapters;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace FluentInjections.Abstractions
{
    public interface IAppBuilderCore<TAppCore> where TAppCore : IAppCore
    {
        Task<TAppCore> BuildAsync();
    }

    public interface IAppCore
    {
        Task RunAsync();
        Task StopAsync(); // Added StopAsync for lifecycle management
    }

    // Adapter Interfaces

    public interface IConcreteBuilderAdapter<TConcreteBuilder, TConcreteApplication> : IBuilderAdapter<TConcreteApplication>
    {
        TConcreteBuilder ConcreteBuilder { get; } // Expose the concrete builder through the adapter interface
    }

    public interface IConcreteApplicationAdapter<TConcreteApplication> : IApplicationAdapter
    {
        TConcreteApplication ConcreteApplication { get; } // Expose the concrete application through the adapter interface
    }

    // Main Application Builder and Application Interfaces

    public interface IApplicationBuilder<TConcreteBuilder, TConcreteApplication, TConcreteBuilderAdapter, TConcreteApplicationAdapter>
        where TConcreteBuilderAdapter : IConcreteBuilderAdapter<TConcreteBuilder, TConcreteApplication>
        where TConcreteApplicationAdapter : IConcreteApplicationAdapter<TConcreteApplication>
    {
        TConcreteBuilderAdapter InnerAdapter { get; } // Now using the Adapter Interface
        Task<IApplication<TConcreteApplication, TConcreteApplicationAdapter>> BuildAsync(); // Returning IApplication with its adapter type
    }

    public interface IApplication<TConcreteApplication, TConcreteApplicationAdapter>
        where TConcreteApplicationAdapter : IConcreteApplicationAdapter<TConcreteApplication>
    {
        TConcreteApplicationAdapter Adapter { get; } // Now using the Adapter Interface
        Task RunAsync();
        Task StopAsync(); // Added StopAsync for lifecycle management
    }

    // Extension Interfaces

    public interface IWebAppMiddlewareApplicationExtension<TConcreteApplicationAdapter>
            where TConcreteApplicationAdapter : IConcreteApplicationAdapter<WebApplication>
    {
        TConcreteApplicationAdapter UseWebAppMiddleware(Func<ApplicationDelegate, ApplicationDelegate> middleware); // Note: on Application Extension now, returns ApplicationAdapter
    }

    // Application Context and Delegate

    public interface IApplicationContext
    {
        CancellationToken CancellationToken { get; }
    }

    // Base Interfaces (less specific, for broader abstraction if needed in future) - optional for now, but good for design thought
    public interface IBuilderAdapter<TApplication>
    {
        Task<TApplication> BuildAsync();
    }

    public interface IApplicationAdapter
    {
        Task RunAsync();
    }
}
