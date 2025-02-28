// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions;
using FluentInjections.Abstractions.Adapters;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

using ILoggerFactoryProvider = FluentInjections.Logging.ILoggerFactoryProvider;

namespace FluentInjections.Adapters.AspNetCore
{
    public class CoreWebApplicationAdapter :
        IConcreteApplicationAdapter<IApplicationBuilder>,
        IAppCore,
        IMiddlewareCapableApplicationAdapter // Implement IMiddlewareCapableApplicationAdapter now
    {
        private readonly WebApplication _innerApplication;
        private readonly CoreWebApplicationBuilderAdapter _builderAdapter;

        public CoreWebApplicationAdapter(IApplicationBuilder application, CoreWebApplicationBuilderAdapter builderAdapter)
        {
            _innerApplication = application as WebApplication ?? throw new ArgumentException("Builder must be a WebApplication", nameof(application));
            _builderAdapter = builderAdapter ?? throw new ArgumentNullException(nameof(builderAdapter));
        }
        // I had to add some properties
        public IApplicationBuilder ConcreteApplication => _innerApplication;
        public ILoggerFactoryProvider? LoggerFactoryProvider => _builderAdapter.LoggerFactoryProvider;

        public IBuilderAdapter<WebApplication> Adapter => (IBuilderAdapter<WebApplication>)_builderAdapter;

        public Task RunAsync(CancellationToken? cancellationToken = null)
        {
            if (cancellationToken.HasValue)
            {
                return _innerApplication.RunAsync(cancellationToken.Value);
            }
            else
            {
                return _innerApplication.RunAsync();
            }
        }

        public Task StopAsync(CancellationToken? cancellationToken = null)
        {
            if (cancellationToken.HasValue)
            {
                return _innerApplication.StopAsync(cancellationToken.Value);
            }
            else
            {
                return _innerApplication.StopAsync();
            }
        }

        public void RegisterMiddleware(Func<ApplicationDelegate, ApplicationDelegate> middleware)
        {
            var middlewareAdapter = new AspNetCoreMiddlewareAdapter(middleware);
            _innerApplication.Use(middlewareAdapter.ToAspNetCoreMiddleware());
        }

        // **Remove or Deprecate UseMiddleware method (as it's now in the interface and implemented as RegisterMiddleware)**
        // You can either remove the `UseMiddleware` method from CoreWebApplicationAdapter completely,
        // OR, you can keep it and mark it as [Obsolete] to indicate that the preferred way is to use the abstract `IApplicationMiddlewareExtension` on IApplication.
        // For now, let's REMOVE it to enforce the new pattern.

        // REMOVED: public CoreWebApplicationAdapter UseMiddleware(Func<ApplicationDelegate, ApplicationDelegate> middleware) { ... }

    }
}
