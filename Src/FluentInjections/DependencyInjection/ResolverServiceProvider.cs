// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions;
using FluentInjections.Exceptions;
using FluentInjections.Validation;

namespace FluentInjections.DependencyInjection
{
    internal class ResolverServiceProvider<TComponent> : IServiceProvider
        where TComponent : IComponent
    {
        private readonly IComponentResolver<TComponent> _resolver;
        private readonly Func<IServiceProvider, CancellationToken, ValueTask<object>> _serviceProviderFactory;
        private readonly IServiceProvider _advancedServiceProvider;
        private readonly Dictionary<Type, object?> _resolvedServices = new Dictionary<Type, object?>();

        public ResolverServiceProvider(IComponentResolver<TComponent> resolver, Func<IServiceProvider, CancellationToken, ValueTask<object>> serviceProviderFactory, IServiceProvider? advancedServiceProvider = null)
        {
            _resolver = resolver;
            _serviceProviderFactory = serviceProviderFactory;
            _advancedServiceProvider = advancedServiceProvider ?? new DefaultServiceProvider();
        }

        public object? GetService(Type serviceType)
        {
            Guard.NotNull(serviceType, nameof(serviceType));

            if (_resolvedServices.TryGetValue(serviceType, out object? cachedService))
            {
                return cachedService;
            }

            try
            {
                object? resolvedService = _resolver.ResolveSingleAsync<object>(
                    (descriptor, ct) => ValueTask.FromResult(descriptor.ContractType == serviceType)).GetAwaiter().GetResult();
                _resolvedServices[serviceType] = resolvedService;
                return resolvedService;
            }
            catch (ComponentResolutionException)
            {
                object? advancedService = _advancedServiceProvider.GetService(serviceType);
                _resolvedServices[serviceType] = advancedService;
                return advancedService;
            }
        }

        private class DefaultServiceProvider : IServiceProvider
        {
            public object? GetService(Type serviceType) => null;
        }
    }
}
