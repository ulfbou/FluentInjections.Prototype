// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Collections;
using FluentInjections.Validation;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace FluentInjections;

public abstract class InjectionBuilderBase<TBuilder> : IInjectionBuilder<TBuilder>
    where TBuilder : class, IApplicationBuilder<TBuilder>
{
    protected readonly IServiceProvider _internalServiceProvider;
    protected readonly TBuilder? _builder;
    protected readonly IServiceProviderFactory? _serviceProviderFactory;
    protected readonly IServiceCollection _services; // Add _services here
    protected readonly IApplicationBuilderExecutor<TBuilder> _applicationBuilderExecutor;
    protected readonly IModuleManagerHandler _moduleManagerHandler;
    protected readonly IServiceProvider _innerProvider; // Renamed for clarity
    protected readonly CancellationTokenSource _cancellationTokenSource;
    protected IServiceProvider? _serviceProvider;
    protected IServiceCollection? _externalServices;
    protected Action<AssemblyCollection> _configureMiddlewaresAction;
    protected Action<AssemblyCollection> _configureLifecyclesAction;
    protected Action<AssemblyCollection> _configureServicesAction;

    protected InjectionBuilderBase(IServiceProvider internalServiceProvider, TBuilder builder, IServiceProviderFactory serviceProviderFactory)
    {
        _internalServiceProvider = internalServiceProvider;
        _builder = builder;
        _serviceProviderFactory = serviceProviderFactory;
        _services = new ServiceCollection();
        _innerProvider = _internalServiceProvider.CreateScope().ServiceProvider;
        _applicationBuilderExecutor = _innerProvider.GetRequiredService<IApplicationBuilderExecutor<TBuilder>>();
        _moduleManagerHandler = _innerProvider.GetRequiredService<IModuleManagerHandler>();
        _cancellationTokenSource = new CancellationTokenSource();
        _configureMiddlewaresAction = _ => { };
        _configureLifecyclesAction = _ => { };
        _configureServicesAction = _ => { };

        SetupInternalServices();
    }

    protected virtual Task RegisterInternalModulesAsync(CancellationToken cancellationToken = default)
    {
        var moduleRegistry = _innerProvider.GetRequiredService<IModuleRegistry>();
        // Register common modules here. Application-specific modules go to derived class.

        return Task.CompletedTask;
    }

    public IInjectionBuilder<TBuilder> WithMiddlewares(Action<AssemblyCollection> configure)
    {
        _configureMiddlewaresAction += configure;
        return this;
    }

    public IInjectionBuilder<TBuilder> WithLifecycles(Action<AssemblyCollection> configure)
    {
        _configureLifecyclesAction += configure;
        return this;
    }

    public IInjectionBuilder<TBuilder> WithServices(Action<AssemblyCollection> configure)
    {
        _configureServicesAction += configure;
        return this;
    }

    public abstract Task<IApplication<TBuilder>> BuildAsync(CancellationToken cancellationToken);
    protected abstract Task RegisterExternalModulesAsync(CancellationToken cancellationToken);

    protected virtual void SetupInternalServices()
    {
        // Register common services (if any) here. Web-specific services go to derived class.
    }

    public void Dispose()
    {
        if (_serviceProvider is IDisposable disposableProvider)
        {
            disposableProvider.Dispose();
        }

        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
    }
}
