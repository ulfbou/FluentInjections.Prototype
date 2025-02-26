// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions;

using Microsoft.Extensions.Logging;

namespace FluentInjections.Adapters.AspNetCore
{
    internal class DefaultApplication<TConcreteApplication, TConcreteApplicationAdapter> :
            IApplication<TConcreteApplication, TConcreteApplicationAdapter>,
            IApplicationMiddlewareExtension<TConcreteApplicationAdapter>
        where TConcreteApplicationAdapter : IConcreteApplicationAdapter<TConcreteApplication>
    {
        private readonly TConcreteApplicationAdapter _adapter;
        public ILoggerFactory LoggerFactory => Adapter.LoggerFactoryProvider?.GetLoggerFactory() ?? Logging.NullLoggerFactory.Instance;
        public TConcreteApplication ConcreteApplication { get; }
        private readonly List<Func<ApplicationDelegate, ApplicationDelegate>> _middlewareDelegates = new List<Func<ApplicationDelegate, ApplicationDelegate>>();


        public DefaultApplication(TConcreteApplicationAdapter adapter, TConcreteApplication concreteApplication)
        {
            _adapter = adapter ?? throw new ArgumentNullException(nameof(adapter));
            ConcreteApplication = concreteApplication ?? throw new ArgumentNullException(nameof(concreteApplication));
        }

        public TConcreteApplicationAdapter Adapter => _adapter;

        public TConcreteApplicationAdapter UseMiddleware(Func<ApplicationDelegate, ApplicationDelegate> middleware)
        {
            if (middleware == null)
            {
                throw new ArgumentNullException(nameof(middleware));
            }
            _middlewareDelegates.Add(middleware); // Keep storing middleware delegates in DefaultApplication (for potential future use, or remove if not needed)

            // **Use IMiddlewareCapableApplicationAdapter Interface for Middleware Registration**
            if (_adapter is IMiddlewareCapableApplicationAdapter middlewareAdapter) // Check for interface
            {
                middlewareAdapter.RegisterMiddleware(middleware); // Call the adapter's RegisterMiddleware method
            }
            else
            {
                // Handle cases where the adapter is NOT middleware-capable (optional):
                // - Do nothing (middleware registration is just not supported for this adapter type)
                // - Throw a NotSupportedException if middleware is expected for all adapter types.
                LoggerFactory.CreateLogger<DefaultApplication<TConcreteApplication, TConcreteApplicationAdapter>>().LogWarning($"Middleware registration is not supported for Application Adapter of type: {_adapter.GetType().FullName}");
            }
            return Adapter;
        }

        public async Task RunAsync(CancellationToken? cancellationToken = null)
        {
            ApplicationDelegate pipeline = context =>
            {
                // **Terminal Delegate: End of Pipeline**
                // When the pipeline reaches the end, simply delegate to the adapter's RunAsync to actually run the concrete application.
                return _adapter.RunAsync(cancellationToken);
            };

            // **Build the Middleware Pipeline by Chaining Delegates in Reverse Order**
            // Iterate through _middlewareDelegates in reverse to build the pipeline in the correct order of execution (outermost to innermost).
            for (int i = _middlewareDelegates.Count - 1; i >= 0; i--)
            {
                var middleware = _middlewareDelegates[i];
                ApplicationDelegate next = pipeline; // Current 'pipeline' becomes 'next' for the current middleware
                pipeline = context => middleware(next)(context); // Chain the middleware: middleware(next)(context)
            }

            // **Execute the Pipeline:**
            // Start the pipeline execution by invoking the first middleware (which is now 'pipeline') with a *null* ApplicationContext for now.
            // We might need to create a more concrete ApplicationContext later if needed for general use cases.
            await pipeline(null!); // Pass null ApplicationContext for now.  Consider creating a default context if needed.
        }

        public async Task StopAsync(CancellationToken? cancellationToken = null)
        {
            await _adapter.StopAsync(cancellationToken);
        }
    }
}
