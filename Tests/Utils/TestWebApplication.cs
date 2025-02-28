// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;

namespace FluentInjections.Tests.Utils
{
    public class TestWebApplication : IWebApplication, IHost // Implement both interfaces
    {
        private readonly IServiceProvider _services;
        private readonly List<Func<RequestDelegate, RequestDelegate>> _middleware = new List<Func<RequestDelegate, RequestDelegate>>();
        private RequestDelegate _pipeline;

        public IServiceProvider Services => _services;

        public TestWebApplication(IServiceCollection services)
        {
            _services = services.BuildServiceProvider();
            _pipeline = BuildRequestPipeline(EmptyRequestDelegate);
        }

        public void Use(Func<RequestDelegate, RequestDelegate> middleware) // Implement Use for middleware registration
        {
            _middleware.Add(middleware);
            UpdateRequestPipeline();
        }

        private RequestDelegate BuildRequestPipeline(RequestDelegate terminalDelegate)
        {
            RequestDelegate pipeline = terminalDelegate;
            for (int i = _middleware.Count - 1; i >= 0; i--)
            {
                var middleware = _middleware[i];
                RequestDelegate next = pipeline;
                pipeline = context => middleware(next)(context);
            }
            return pipeline;
        }

        private void UpdateRequestPipeline()
        {
            _pipeline = BuildRequestPipeline(EmptyRequestDelegate);
        }

        private static Task EmptyRequestDelegate(HttpContext context)
        {
            return Task.CompletedTask; // Terminal delegate - does nothing
        }

        // **Corrected RunAsync for Testing - Executes pipeline and COMPLETS**
        public Task RunAsync(CancellationToken cancellationToken = default)
        {
            var httpContext = new DefaultHttpContext(); // Create a fake HttpContext
            return _pipeline(httpContext); // Execute the middleware pipeline and RETURN
        }

        public void Dispose()
        {
            // No resources to dispose in this fake for now
        }

        public Task StartAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask; // No-op for testing
        }

        public Task StopAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask; // No-op for testing
        }
    }
}
