// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using System.Reflection;

namespace FluentInjections;

public abstract class InternalBuilder<TBuilder>
    where TBuilder : class, IApplicationBuilder<TBuilder>
{
    protected readonly IServiceProvider _innerProvider;
    protected TBuilder _builder;
    protected readonly IServiceCollection _services; // Developer's services
    protected readonly IServiceProviderFactory _serviceProviderFactory;
    protected readonly IAssemblyManager _assemblyManager;
    protected readonly IModuleManager _moduleManager;
    protected readonly IApplicationBuilderExecutor<TBuilder> _applicationBuilderExecutor;
    protected readonly CancellationTokenSource _cancellationTokenSource = new();

    protected InternalBuilder(
        TBuilder builder,
        IServiceProviderFactory? serviceProviderFactory,
        IServiceProvider internalServiceProvider)
    {
        _builder = builder;
        _innerProvider = internalServiceProvider;
        _serviceProviderFactory = serviceProviderFactory ?? throw new ArgumentNullException(nameof(serviceProviderFactory));
        _services = new ServiceCollection();
        _assemblyManager = _innerProvider.GetRequiredService<IAssemblyManager>();
        _moduleManager = _innerProvider.GetRequiredService<IModuleManager>();
        _applicationBuilderExecutor = _innerProvider.GetRequiredService<IApplicationBuilderExecutor<TBuilder>>();
    }

    public virtual async Task DiscoverServices(IServiceProvider serviceProvider, IEnumerable<Assembly> assemblies, CancellationToken cancellationToken = default)
    {
        await RegisterInternalModulesAsync(cancellationToken).ConfigureAwait(false);
        await _assemblyManager.DiscoverModulesAsync(serviceProvider, cancellationToken).ConfigureAwait(false);
    }

    protected virtual async Task RegisterInternalModulesAsync(CancellationToken cancellationToken = default)
    {
        // Get IModuleRegistry from the *internal* service provider
        var moduleRegistry = _innerProvider.GetRequiredService<IModuleRegistry>();

        var module = new InternalServiceInjectionModule<TBuilder>(_builder ?? throw new InvalidOperationException("Builder not set"));
        await moduleRegistry.RegisterAsync<InternalServiceInjectionModule<TBuilder>, IServiceConfigurator>(module, cancellationToken);
    }
}
