// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions;
using FluentInjections.DependencyInjection;
using FluentInjections.Registration;
using FluentInjections.Validation;

using Microsoft.Extensions.Logging;

namespace FluentInjections.Internal.Registration
{
    internal class ComponentBuilder<TComponent, TContract> : IComponentBuilder<TComponent, TContract>
        where TComponent : IComponent
    {
        private readonly IComponentRegistration<TComponent, TContract> _registration;
        private readonly ILogger _logger;
        private bool _disposed;

        public ComponentBuilder(IComponentRegistration<TComponent, TContract> registration, ILoggerFactory loggerFactory)
        {
            Guard.NotNull(registration, nameof(registration));
            Guard.NotNull(loggerFactory, nameof(loggerFactory));
            _registration = registration;
            _logger = loggerFactory.CreateLogger(GetType().Name);
        }

        public IComponentBuilder<TComponent, TContract> AsScoped()
        {
            ValidateLifetime(ComponentLifetime.Scoped);
            _registration.Lifetime = ComponentLifetime.Scoped;
            return this;
        }

        public IComponentBuilder<TComponent, TContract> AsSingleton()
        {
            ValidateLifetime(ComponentLifetime.Singleton);
            _registration.Lifetime = ComponentLifetime.Singleton;
            return this;
        }

        public IComponentBuilder<TComponent, TContract> AsTransient()
        {
            ValidateLifetime(ComponentLifetime.Transient);
            _registration.Lifetime = ComponentLifetime.Transient;
            return this;
        }

        public IComponentBuilder<TComponent, TContract> To<TImplementation>() where TImplementation : class, TContract
        {
            ValidateType(resolutionType: typeof(TImplementation));

            _registration.ResolutionType = typeof(TImplementation);
            return this;
        }

        public IComponentBuilder<TComponent, TContract> ToSelf()
        {
            ValidateType(resolutionType: typeof(TContract));
            _registration.ResolutionType = typeof(TContract);
            return this;
        }

        public IComponentBuilder<TComponent, TContract> UsingFactory(Func<IComponentResolver<TComponent>, CancellationToken, ValueTask<TContract>> factory)
        {
            ValidateType(factory: factory);
            _registration.Factory = factory;
            return this;
        }

        public IComponentBuilder<TComponent, TContract> UsingInstance(TContract instance)
        {
            ValidateType(instance: instance);
            _registration.Instance = instance;
            return this;
        }

        public IComponentBuilder<TComponent, TContract> WithLifetime(ComponentLifetime lifetime)
        {
            ValidateLifetime(lifetime);
            _registration.Lifetime = lifetime;
            return this;
        }

        public IComponentBuilder<TComponent, TContract> WithMetadata(string key, object? value)
        {
            Guard.NotNullOrWhiteSpace(key, nameof(key));

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Component {Component} metadata key {Key} set to {Value}", _registration.Alias, key, value);
            }

            _registration.Metadata[key] = value;
            return this;
        }

        public IComponentBuilder<TComponent, TContract> WithParameters(object parameters)
        {
            Guard.NotNull(parameters, nameof(parameters));

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Component {Component} parameters registered.", _registration.Alias);
            }

            _registration.Parameters = parameters;
            return this;
        }

        public IComponentBuilder<TComponent, TContract> Configure(Func<TContract, CancellationToken, ValueTask> configure)
        {
            Guard.NotNull(configure, nameof(configure));

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Component {Component} registered a configuration delegate.", _registration.Alias);
            }

            _registration.Configure += configure;
            return this;
        }

        public void Dispose()
        {
            if (_disposed) return;

            _disposed = true;
            _registration.Dispose();
        }

        #region Validation
        private void ValidateType(
            Type? resolutionType = null,
            TContract? instance = default,
            Func<IComponentResolver<TComponent>, CancellationToken, ValueTask<TContract>>? factory = null)
        {
            if (resolutionType is null && instance is null && factory is null)
            {
                throw new ArgumentException("At least one of resolutionType, instance or factory must be provided.");
            }

            if (resolutionType is not null)
            {
                if (_registration.ResolutionType is not null && _registration.ResolutionType != resolutionType)
                {
                    if (_logger.IsEnabled(LogLevel.Warning))
                    {
                        _logger.LogWarning("Component {Component} already registered as {ResolutionType}. Overriding to {ResolutionType}", _registration.Alias, _registration.ResolutionType, resolutionType);
                    }
                }

                if (_registration.Instance is not null)
                {
                    if (_logger.IsEnabled(LogLevel.Warning))
                    {
                        _logger.LogWarning("Component {Component} already registered as instance. Overriding to {ResolutionType}", _registration.Alias, resolutionType);
                    }

                    _registration.Instance = default;
                }

                if (_registration.Factory is not null)
                {
                    if (_logger.IsEnabled(LogLevel.Warning))
                    {
                        _logger.LogWarning("Component {Component} already registered with a factory. Overriding to {ResolutionType}", _registration.Alias, resolutionType);
                    }

                    _registration.Factory = default;
                }

                _logger.LogDebug("Component {Component} registered as {ResolutionType}", _registration.Alias, resolutionType);
            }

            if (instance is not null)
            {
                if (_registration.Instance is not null && !_registration.Instance.Equals(instance))
                {
                    if (_logger.IsEnabled(LogLevel.Warning))
                    {
                        _logger.LogWarning("Component {Component} already registered as instance. Overriding to new instance", _registration.Alias);
                    }
                }

                if (_registration.ResolutionType is not null)
                {
                    if (_logger.IsEnabled(LogLevel.Warning))
                    {
                        _logger.LogWarning("Component {Component} already registered as {ResolutionType}. Overriding to instance", _registration.Alias, _registration.ResolutionType);
                    }

                    _registration.ResolutionType = default;
                }

                if (_registration.Factory is not null)
                {
                    if (_logger.IsEnabled(LogLevel.Warning))
                    {
                        _logger.LogWarning("Component {Component} already registered with a factory. Overriding to instance", _registration.Alias);
                    }

                    _registration.Factory = default;
                }

                _logger.LogDebug("Component {Component} registered as instance", _registration.Alias);
            }

            if (factory is not null)
            {
                if (_registration.Factory is not null && _registration.Factory != factory)
                {
                    if (_logger.IsEnabled(LogLevel.Warning))
                    {
                        _logger.LogWarning("Component {Component} already registered with a factory. Overriding to new factory", _registration.Alias);
                    }
                }

                if (_registration.ResolutionType is not null)
                {
                    if (_logger.IsEnabled(LogLevel.Warning))
                    {
                        _logger.LogWarning("Component {Component} already registered as {ResolutionType}. Overriding to factory", _registration.Alias, _registration.ResolutionType);
                    }

                    _registration.ResolutionType = default;
                }

                if (_registration.Instance is not null)
                {
                    if (_logger.IsEnabled(LogLevel.Warning))
                    {
                        _logger.LogWarning("Component {Component} already registered as instance. Overriding to factory", _registration.Alias);
                    }

                    _registration.Instance = default;
                }

                _logger.LogDebug("Component {Component} registered with a factory", _registration.Alias);
            }
        }

        private void ValidateLifetime(ComponentLifetime lifetime)
        {
            if (_registration.Lifetime != lifetime)
            {
                if (_logger.IsEnabled(LogLevel.Warning))
                {
                    _logger.LogWarning("Component {Component} already registered with lifetime {Lifetime}. Overriding to {Lifetime}", _registration.Alias, _registration.Lifetime, lifetime);
                }
            }
            else if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug("Component {Component} registered with lifetime {Lifetime}", _registration.Alias, lifetime);
            }
        }
        #endregion
    }
}
