// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions;
using FluentInjections.Abstractions.Adapters;

using Microsoft.Extensions.Logging;
namespace FluentInjections.Adapters
{
    internal class DefaultApplication<TConcreteApplication, TConcreteApplicationAdapter> :
        IApplicationAdapter<TConcreteApplication, TConcreteApplicationAdapter>,
        IApplicationMiddlewareExtension<TConcreteApplicationAdapter>
            where TConcreteApplication : notnull
            where TConcreteApplicationAdapter : notnull, IConcreteApplicationAdapter<TConcreteApplication>
    {
        private readonly TConcreteApplicationAdapter _adapter;
        public ILoggerFactory LoggerFactory => Adapter.LoggerFactoryProvider?.GetLoggerFactory() ?? Logging.NullLoggerFactory.Instance;
        public TConcreteApplication ConcreteApplication { get; }
        private readonly List<Func<ApplicationDelegate, ApplicationDelegate>> _middlewareDelegates = new List<Func<ApplicationDelegate, ApplicationDelegate>>();


        public DefaultApplication(TConcreteApplicationAdapter adapter, TConcreteApplication concreteApplication)
        {
            _adapter = adapter;
            ConcreteApplication = concreteApplication ?? throw new ArgumentNullException(nameof(concreteApplication));
        }

        public TConcreteApplicationAdapter Adapter => _adapter;

        public TConcreteApplicationAdapter UseMiddleware(Func<ApplicationDelegate, ApplicationDelegate> middleware)
        {
            if (middleware == null)
            {
                throw new ArgumentNullException(nameof(middleware));
            }

            _middlewareDelegates.Add(middleware);

            if (_adapter is IMiddlewareCapableApplicationAdapter middlewareAdapter) // Check for interface
            {
                middlewareAdapter.RegisterMiddleware(middleware); // Call the adapter's RegisterMiddleware method
            }
            else
            {
                // TODO: Decide how to Handle cases where the adapter is NOT middleware-capable (optional):
                // - Do nothing (middleware registration is just not supported for this adapter type)
                // - Throw a NotSupportedException if middleware is expected for all adapter types.
                LoggerFactory.CreateLogger<DefaultApplication<TConcreteApplication, TConcreteApplicationAdapter>>().LogWarning($"Middleware registration is not supported for Application Adapter of type: {_adapter.GetType().FullName}");
            }

            return Adapter;
        }

        public Task RunAsync(CancellationToken cancellationToken = default)
        {
            if (cancellationToken == default)
            {
                return _adapter.RunAsync();
            }
            else
            {
                return _adapter.RunAsync(cancellationToken);
            }
        }

        public async Task StopAsync(CancellationToken? cancellationToken = null)
        {
            await _adapter.StopAsync(cancellationToken);
        }
    }
}
