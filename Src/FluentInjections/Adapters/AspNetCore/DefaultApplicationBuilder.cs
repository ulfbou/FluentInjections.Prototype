// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions;
using FluentInjections.Configuration;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using NullLoggerFactory = FluentInjections.Logging.NullLoggerFactory;

namespace FluentInjections.Adapters.AspNetCore
{
    internal class DefaultApplicationBuilder<TConcreteBuilder, TConcreteApplication, TConcreteBuilderAdapter, TConcreteApplicationAdapter> :
        IApplicationBuilder<TConcreteBuilder, TConcreteApplication, TConcreteBuilderAdapter, TConcreteApplicationAdapter>
            where TConcreteBuilderAdapter : IConcreteBuilderAdapter<TConcreteBuilder, TConcreteApplication>
            where TConcreteApplicationAdapter : IConcreteApplicationAdapter<TConcreteApplication>
    {
        private readonly TConcreteBuilderAdapter _innerAdapter;
        private readonly Dictionary<Type, Func<TConcreteApplication, TConcreteBuilderAdapter, TConcreteApplicationAdapter>> _applicationAdapterFactories = new Dictionary<Type, Func<TConcreteApplication, TConcreteBuilderAdapter, TConcreteApplicationAdapter>>();

        public DefaultApplicationBuilder(TConcreteBuilderAdapter innerAdapter)
        {
            _innerAdapter = innerAdapter ?? throw new ArgumentNullException(nameof(innerAdapter));
        }

        public TConcreteBuilderAdapter InnerAdapter => _innerAdapter;

        public IConfiguration Configuration => _innerAdapter.ConfigurationProvider?.GetConfiguration() ?? NullConfiguration.Instance;
        public ILoggerFactory LoggerFactory => _innerAdapter.LoggerFactoryProvider?.GetLoggerFactory() ?? NullLoggerFactory.Instance;

        public async Task<IApplication<TConcreteApplication, TConcreteApplicationAdapter>> BuildAsync()
        {
            var concreteApplication = await _innerAdapter.BuildAsync();
            var applicationAdapter = CreateApplicationAdapter(concreteApplication); // Use the factory method
            return new DefaultApplication<TConcreteApplication, TConcreteApplicationAdapter>(applicationAdapter, concreteApplication);
        }

        private TConcreteApplicationAdapter CreateApplicationAdapter(TConcreteApplication concreteApplication)
        {
            Type adapterType = typeof(TConcreteApplicationAdapter);
            if (_applicationAdapterFactories.TryGetValue(adapterType, out var factory))
            {
                // Directly invoke the factory delegate - type-safe due to generic dictionary and registration
                return factory.Invoke(concreteApplication, _innerAdapter);
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
    }
}
