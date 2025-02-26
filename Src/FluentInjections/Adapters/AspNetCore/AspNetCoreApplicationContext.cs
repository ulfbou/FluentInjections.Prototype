// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace FluentInjections.Adapters.AspNetCore
{
    // Concrete Implementation for ASP.NET Core Context (AspNetCoreApplicationContext.cs)
    public class AspNetCoreApplicationContext : IApplicationContext
    {
        private readonly HttpContext _httpContext;

        public AspNetCoreApplicationContext(HttpContext httpContext)
        {
            _httpContext = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
        }

        public HttpContext HttpContext => _httpContext; // Expose the underlying HttpContext if needed

        public CancellationToken CancellationToken => _httpContext.RequestAborted;
    }

    internal class AspNetCoreMiddlewareAdapter
    {
        private readonly Func<ApplicationDelegate, ApplicationDelegate> _abstractMiddleware;

        public AspNetCoreMiddlewareAdapter(Func<ApplicationDelegate, ApplicationDelegate> abstractMiddleware)
        {
            _abstractMiddleware = abstractMiddleware ?? throw new ArgumentNullException(nameof(_abstractMiddleware));
        }

        public Func<RequestDelegate, RequestDelegate> ToAspNetCoreMiddleware()
        {
            return nextRequestDelegate =>
            {
                return async httpContext =>
                {
                    var appContext = new AspNetCoreApplicationContext(httpContext);
                    ApplicationDelegate adaptedNextDelegate = abstractAppContext =>
                    {
                        return nextRequestDelegate(((AspNetCoreApplicationContext)abstractAppContext).HttpContext);
                    };
                    await _abstractMiddleware(adaptedNextDelegate)(appContext);
                };
            };
        }
    }

    public class WebApplicationBuilderAdapter : IConcreteBuilderAdapter<WebApplicationBuilder, WebApplication>
    {
        private readonly WebApplicationBuilder _innerBuilder;

        public WebApplicationBuilderAdapter(WebApplicationBuilder builder)
        {
            _innerBuilder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        public WebApplicationBuilder ConcreteBuilder => _innerBuilder; // Implementing the interface property

        public async Task<WebApplication> BuildAsync() // Implementing IBuilderAdapter<WebApplication>.BuildAsync
        {
            return await Task.FromResult(_innerBuilder.Build()); // Example, might need to be sync in WebAppBuilder
        }
    }
    public class WebApplicationAdapter : IConcreteApplicationAdapter<WebApplication>
    {
        private readonly WebApplication _innerApplication;
        private readonly WebApplicationBuilderAdapter _builderAdapter; // Store the builder adapter for relationship (optional, depending on needs)

        public WebApplicationAdapter(WebApplication application, WebApplicationBuilderAdapter builderAdapter)
        {
            _innerApplication = application ?? throw new ArgumentNullException(nameof(application));
            _builderAdapter = builderAdapter ?? throw new ArgumentNullException(nameof(builderAdapter)); // Optional
        }

        public WebApplication ConcreteApplication => _innerApplication; // Implementing interface property
        public IBuilderAdapter<WebApplication> Adapter => _builderAdapter; // Implementing IApplication<WebApplication>.Adapter

        public async Task RunAsync() // Implementing IApplicationAdapter.RunAsync
        {
            await _innerApplication.RunAsync();
        }
    }

    public class CoreWebApplicationBuilderAdapter :
        IConcreteBuilderAdapter<WebApplicationBuilder, WebApplication>,
        IAppBuilderCore<CoreWebApplicationAdapter> // Builds CoreWebApplicationAdapter
    {
        private readonly WebApplicationBuilder _innerBuilder;

        public CoreWebApplicationBuilderAdapter(WebApplicationBuilder builder)
        {
            _innerBuilder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        public WebApplicationBuilder ConcreteBuilder => _innerBuilder; // Implementing interface property

        public Task<CoreWebApplicationAdapter> BuildAsync() // Builds CoreWebApplicationAdapter now
        {
            var webApplication = _innerBuilder.Build();
            return Task.FromResult(new CoreWebApplicationAdapter(webApplication, this)); // Pass builder adapter for relation
        }

        Task<WebApplication> IBuilderAdapter<WebApplication>.BuildAsync() => throw new NotImplementedException();
    }
    public class CoreWebApplicationAdapter :
        IConcreteApplicationAdapter<WebApplication>,
        IAppCore,
        IWebAppMiddlewareApplicationExtension<CoreWebApplicationAdapter> // Correct Interface!
    {
        private readonly WebApplication _innerApplication;
        private readonly CoreWebApplicationBuilderAdapter _builderAdapter; // Hold builder adapter for relation

        public WebApplication ConcreteApplication => _innerApplication; // Implementing interface property

        public CoreWebApplicationAdapter(WebApplication application, CoreWebApplicationBuilderAdapter builderAdapter)
        {
            _innerApplication = application ?? throw new ArgumentNullException(nameof(application));
            _builderAdapter = builderAdapter ?? throw new ArgumentNullException(nameof(builderAdapter));
        }

        public CoreWebApplicationAdapter UseWebAppMiddleware(Func<ApplicationDelegate, ApplicationDelegate> middleware) // Correct Return Type!
        {
            var middlewareAdapter = new AspNetCoreMiddlewareAdapter(middleware); // Now using the separate internal class
            _innerApplication.Use(middlewareAdapter.ToAspNetCoreMiddleware());
            return this; // Correct Return! Return application adapter for chaining
        }

        public async Task RunAsync()
        {
            await _innerApplication.RunAsync();
        }

        public async Task StopAsync()
        {
            await _innerApplication.StopAsync(); // Or handle stop logic if needed
        }
    }

    public class CoreHostBuilderAdapter : IAppBuilderCore<CoreHostApplicationAdapter> // Builds CoreHostApplicationAdapter
    {
        private readonly IHostBuilder _innerBuilder;

        public CoreHostBuilderAdapter(IHostBuilder builder)
        {
            _innerBuilder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        public Task<CoreHostApplicationAdapter> BuildAsync() // Builds CoreHostApplicationAdapter
        {
            var host = _innerBuilder.Build();
            return Task.FromResult(new CoreHostApplicationAdapter(host, this)); // Pass builder adapter for relation
        }

        // No IWebAppMiddlewareBuilderExtension implementation - HostBuilder doesn't have web middleware
        // Could potentially have other extensions later for HostBuilder-specific features (e.g., service config extensions)
    }

    public class CoreHostApplicationAdapter : IAppCore // Implements Core Application
    {
        private readonly IHost _innerHost;
        private readonly CoreHostBuilderAdapter _builderAdapter; // Hold builder adapter for relation

        public CoreHostApplicationAdapter(IHost host, CoreHostBuilderAdapter builderAdapter)
        {
            _innerHost = host ?? throw new ArgumentNullException(nameof(host));
            _builderAdapter = builderAdapter ?? throw new ArgumentNullException(nameof(builderAdapter));
        }

        public async Task RunAsync()
        {
            await _innerHost.RunAsync();
        }

        public async Task StopAsync()
        {
            await _innerHost.StopAsync(); // Or handle stop logic if needed
        }
    }
}
