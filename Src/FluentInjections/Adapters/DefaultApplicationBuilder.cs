// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions;
using FluentInjections.Abstractions.Adapters;
using FluentInjections.Configuration;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using NullLoggerFactory = FluentInjections.Logging.NullLoggerFactory;

namespace FluentInjections.Adapters
{
    internal class DefaultApplicationBuilder<TConcreteBuilder, TConcreteApplication, TConcreteBuilderAdapter, TConcreteApplicationAdapter> :
        IApplicationBuilder<TConcreteBuilder, TConcreteApplication, TConcreteBuilderAdapter, TConcreteApplicationAdapter>
            where TConcreteBuilder : notnull
            where TConcreteApplication : notnull
            where TConcreteBuilderAdapter : IConcreteBuilderAdapter<TConcreteBuilder, TConcreteApplication>
            where TConcreteApplicationAdapter : IConcreteApplicationAdapter<TConcreteApplication>
    {
        private readonly TConcreteBuilderAdapter _innerBuilderAdapter;
        private readonly TConcreteApplicationAdapter _innerApplicationAdapter;
        private readonly Dictionary<Type, Func<TConcreteApplication, TConcreteBuilderAdapter, TConcreteApplicationAdapter>> _applicationAdapterFactories = new Dictionary<Type, Func<TConcreteApplication, TConcreteBuilderAdapter, TConcreteApplicationAdapter>>();

        public DefaultApplicationBuilder(TConcreteBuilderAdapter innerBuilderAdapter, TConcreteApplicationAdapter innerApplicationAdapter)
        {
            _innerBuilderAdapter = innerBuilderAdapter ?? throw new ArgumentNullException(nameof(innerBuilderAdapter));
            _innerApplicationAdapter = innerApplicationAdapter ?? throw new ArgumentNullException(nameof(innerApplicationAdapter));
        }

        public TConcreteBuilderAdapter InnerAdapter => _innerBuilderAdapter;

        public IConfiguration Configuration => _innerBuilderAdapter.ConfigurationProvider?.GetConfiguration() ?? NullConfiguration.Instance;
        public ILoggerFactory LoggerFactory => _innerBuilderAdapter.LoggerFactoryProvider?.GetLoggerFactory() ?? NullLoggerFactory.Instance;

        public async Task<IApplicationAdapter<TConcreteApplication, TConcreteApplicationAdapter>> BuildAsync(CancellationToken? cancellationToken = null)
        {
            var concreteApplication = await _innerBuilderAdapter.BuildAsync(cancellationToken ?? CancellationToken.None);
            var applicationAdapter = CreateApplicationAdapter(concreteApplication);
            return new DefaultApplication<TConcreteApplication, TConcreteApplicationAdapter>(applicationAdapter, concreteApplication);
        }

        private TConcreteApplicationAdapter CreateApplicationAdapter(TConcreteApplication concreteApplication)
        {
            Type adapterType = typeof(TConcreteApplicationAdapter);
            if (_applicationAdapterFactories.TryGetValue(adapterType, out var factory))
            {
                // Directly invoke the factory delegate - type-safe due to generic dictionary and registration
                return factory.Invoke(concreteApplication, _innerBuilderAdapter);
            }

            throw new InvalidOperationException($"No Application Adapter Factory registered for type: {adapterType.FullName}. You must register a factory using RegisterApplicationAdapterFactory method.");
        }

        public void RegisterApplicationAdapterFactory<TAppAdapter>(Func<TConcreteApplication, TConcreteBuilderAdapter, TAppAdapter> factory)
            where TAppAdapter : TConcreteApplicationAdapter
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            _applicationAdapterFactories[typeof(TAppAdapter)] = (Func<TConcreteApplication, TConcreteBuilderAdapter, TConcreteApplicationAdapter>)(Delegate)factory;
        }

        public Task<IApplicationAdapter<TConcreteApplication, TConcreteApplicationAdapter>> BuildAsync(CancellationToken cancellationToken) => throw new NotImplementedException();
    }
}
