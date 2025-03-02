// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions;
using FluentInjections.Validation;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using System.Runtime.CompilerServices;

namespace FluentInjections.DependencyInjection
{
    public class ComponentRegistry<TComponent> : IComponentRegistry<TComponent>
    where TComponent : IComponent
    {
        private readonly IServiceCollection _services;
        private readonly ILogger<ComponentRegistry<TComponent>> _logger;
        private IServiceProvider? _serviceProvider;
        private readonly Dictionary<Type, Dictionary<string, IComponentRegistration<TComponent>>> _registrations = new(); // Dictionary per TContract

        public ComponentRegistry(ILogger<ComponentRegistry<TComponent>> logger)
        {
            Guard.NotNull(logger, nameof(logger));
            _services = new ServiceCollection();
            _logger = logger;
        }

        public IServiceProvider GetServiceProvider()
        {
            if (_serviceProvider == null)
            {
                BuildServiceProviderInternal();
            }

            return _serviceProvider!;
        }

        private void BuildServiceProviderInternal()
        {
            if (!_registrations.Any())
            {
                _serviceProvider = _services.BuildServiceProvider();
                return;
            }

            foreach (Dictionary<string, IComponentRegistration<TComponent>> contractRegistrations in _registrations.Values)
            {
                foreach (object registrationObj in contractRegistrations.Values)
                {
                    if (registrationObj is IComponentRegistration<TComponent, object> registration)
                    {
                        _services.Add(CreateServiceDescriptor(registration));
                    }
                }
            }

            _serviceProvider = _services.BuildServiceProvider();
        }

        private ServiceDescriptor CreateServiceDescriptor<TContract>(IComponentRegistration<TComponent, TContract> registration)
        {
            if (registration.Instance != null)
            {
                return new ServiceDescriptor(
                    registration.ContractType,
                    registration.Instance);
            }

            if (registration.Factory != null)
            {
                return new ServiceDescriptor(
                    registration.ContractType,
                    (provider) => registration.Factory(provider).AsTask().Result!,
                    registration.Lifetime.ToServiceLifetime());
            }

            if (registration.ResolutionType == null)
            {
                throw new InvalidOperationException($"ResolutionType is required for registration with alias '{registration.Alias}'.");
            }

            return new ServiceDescriptor(
                registration.ContractType,
                sp => sp.GetService(registration.ResolutionType)!,
                registration.Lifetime.ToServiceLifetime());
        }

        public async ValueTask RegisterAsync<TRegistration, TContract>(TRegistration registration, CancellationToken cancellationToken = default)
            where TRegistration : IComponentRegistration<TComponent, TContract>
        {
            Guard.NotNull(registration, nameof(registration));

            if (string.IsNullOrWhiteSpace(registration.Alias))
            {
                registration.Alias = typeof(TContract).FullName ?? typeof(TContract).Name;
            }

            if (!_registrations.ContainsKey(typeof(TContract)))
            {
                _registrations[typeof(TContract)] = new Dictionary<string, IComponentRegistration<TComponent>>();
            }

            if (_registrations[typeof(TContract)].ContainsKey(registration.Alias))
            {
                _logger.LogWarning($"A component with alias '{registration.Alias}' is already registered for contract '{typeof(TContract).FullName}'.");
            }

            _registrations[typeof(TContract)][registration.Alias] = registration;
            _serviceProvider = null; // Invalidate service provider

            await ValueTask.CompletedTask;
        }

        public ValueTask<bool> UnregisterAsync(string? alias = null, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(alias))
                throw new ArgumentException("Alias cannot be null or empty.", nameof(alias));

            bool removed = false;

            foreach (var contractRegistrations in _registrations.Values)
            {
                if (contractRegistrations.Remove(alias))
                {
                    removed = true;
                    break;
                }
            }

            if (removed)
            {
                _serviceProvider = null; // Invalidate service provider
            }

            return new ValueTask<bool>(removed);
        }

        public async IAsyncEnumerable<TDescriptor> GetManyAsync<TDescriptor, TContract>(
                Func<TDescriptor, CancellationToken, ValueTask<bool>>? predicate = null,
                [EnumeratorCancellation] CancellationToken cancellationToken = default)
            where TDescriptor : IComponentDescriptor<TComponent, TContract>
        {
            if (_registrations.TryGetValue(typeof(TContract), out var contractRegistrations))
            {
                foreach (var registrationObj in contractRegistrations.Values)
                {
                    if (registrationObj is TDescriptor descriptor)
                    {
                        if (predicate == null || await predicate(descriptor, cancellationToken))
                        {
                            yield return descriptor;
                        }
                    }
                }
            }
        }

        public async IAsyncEnumerable<TDescriptor> GetManyAsync<TDescriptor, TContract>(IEnumerable<string> aliases, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
            where TDescriptor : IComponentDescriptor<TComponent, TContract>
        {
            if (_registrations.TryGetValue(typeof(TContract), out var contractRegistrations))
            {
                foreach (var alias in aliases)
                {
                    if (await TryGetDescriptorAsync<TDescriptor, TContract>(contractRegistrations, alias, out var descriptor).ConfigureAwait(false))
                    {
                        yield return descriptor!;
                    }
                }
            }

            yield break;
        }

        public async ValueTask<TDescriptor?> GetSingleAsync<TDescriptor, TContract>(string? alias = null, CancellationToken cancellationToken = default)
            where TDescriptor : IComponentDescriptor<TComponent, TContract>
        {
            alias = GetAlias<TContract>(alias);

            if (await TryGetDescriptorAsync<TDescriptor, TContract>(_registrations[typeof(TContract)], alias, out var descriptor).ConfigureAwait(false))
            {
                return descriptor;
            }

            return default;
        }

        public async ValueTask<TDescriptor?> GetSingleAsync<TDescriptor, TContract>(Func<TDescriptor, CancellationToken, ValueTask<bool>> predicate, CancellationToken cancellationToken = default)
            where TDescriptor : IComponentDescriptor<TComponent, TContract>
        {
            if (predicate == null)
            {
                return default;
            }

            if (_registrations.TryGetValue(typeof(TContract), out var contractRegistrations))
            {
                foreach (var registrationObj in contractRegistrations.Values)
                {
                    if (registrationObj is TDescriptor descriptor && await predicate(descriptor, cancellationToken))
                    {
                        return descriptor;
                    }
                }
            }

            return default;
        }

        private ValueTask<bool> TryGetDescriptorAsync<TDescriptor, TContract>(Dictionary<string, IComponentRegistration<TComponent>> contractRegistrations, string alias, out TDescriptor? descriptor)
            where TDescriptor : IComponentDescriptor<TComponent, TContract>
        {
            if (contractRegistrations.TryGetValue(alias, out var registrationObj) &&
                registrationObj is TDescriptor descriptorObj)
            {
                descriptor = descriptorObj;
                return new ValueTask<bool>(true);
            }

            descriptor = default!;
            return new ValueTask<bool>(false);
        }

        private string GetAlias<TContract>(string? alias)
        {
            return string.IsNullOrWhiteSpace(alias) ? typeof(TContract).FullName ?? typeof(TContract).Name : alias!;
        }
    }
}
