// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FluentInjections.DependencyInjection;

public class ComponentResolverBase<TComponent> where TComponent : IComponent
{
    protected readonly IComponentResolver<TComponent>? _innerResolver;
    protected readonly IServiceProvider _serviceProvider;
    protected readonly ILogger _logger;

    public IServiceProvider ServiceProvider => _serviceProvider;


    public ComponentResolverBase(IServiceProvider provider)// Constructor 1: Use provided IServiceProvider (preferred in most cases)
    {
        _serviceProvider = provider ?? throw new ArgumentNullException(nameof(provider));
        _logger = provider.GetService<ILoggerFactory>()?.CreateLogger(GetType())
            ?? throw new InvalidOperationException("Unable to create logger.");
    }


    public ComponentResolverBase(IComponentResolver<TComponent> resolver)// Constructor 2: Iterate to find IServiceProvider (for specific scenarios)
    {
        _innerResolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
        _serviceProvider = GetServiceProvider(resolver);
        _logger = _serviceProvider.GetService<ILoggerFactory>()?.CreateLogger(GetType())
            ?? throw new InvalidOperationException("Unable to create logger.");
    }

    private IServiceProvider GetServiceProvider(IComponentResolver<TComponent> resolver)
    {
        ComponentResolverBase<TComponent>? currentResolver = resolver as ComponentResolverBase<TComponent>;

        while (currentResolver != null)
        {
            if (currentResolver._serviceProvider != null)
            {
                return currentResolver._serviceProvider;
            }

            currentResolver = currentResolver._innerResolver as ComponentResolverBase<TComponent>;
        }

        _logger.LogError("Unable to find an IServiceProvider.");
        throw new InvalidOperationException("Unable to find an IServiceProvider.");
    }
}