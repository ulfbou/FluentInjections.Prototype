// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions.Adapters;
using FluentInjections.Adapters.AspNetCore;

using Microsoft.Extensions.DependencyInjection;

using Microsoft.Extensions.Hosting;

namespace FluentInjections.Adapters
{
    public class DefaultApplicationAdapterFactory :
        IApplicationAdapterFactory<IHost, CoreWebApplicationAdapter> // Concrete types for ASP.NET Core
    {
        private readonly IServiceProvider _serviceProvider;

        public DefaultApplicationAdapterFactory(IServiceProvider serviceProvider) // Inject IServiceProvider
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public CoreWebApplicationAdapter CreateAdapter(IHost concreteApplication)
        {
            // Resolve CoreWebApplicationBuilderAdapter from DI (if needed by CoreWebApplicationAdapter)
            var builderAdapter = _serviceProvider.GetRequiredService<CoreWebApplicationBuilderAdapter>(); // Resolve from DI

            // IMPORTANT: The factory implementation is framework-specific and knows about CoreWebApplicationBuilderAdapter
            return new CoreWebApplicationAdapter(concreteApplication, builderAdapter); // Create CoreWebApplicationAdapter, now with builderAdapter resolved from DI
        }
    }
}
