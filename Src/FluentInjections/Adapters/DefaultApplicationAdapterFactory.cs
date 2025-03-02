// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions;
using FluentInjections.Abstractions.Adapters;
using FluentInjections.Abstractions.Factories;
using FluentInjections.Adapters.AspNetCore;

using Microsoft.Extensions.DependencyInjection;

using Microsoft.Extensions.Hosting;

namespace FluentInjections.Adapters
{
    //public class DefaultApplicationAdapterFactory :
    //    IApplicationAdapterFactory
    //{
    //    private readonly IServiceProvider _serviceProvider;

    //    public DefaultApplicationAdapterFactory(IServiceProvider serviceProvider) // Inject IServiceProvider
    //    {
    //        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    //    }

    //    public IApplication<TBuilder> CreateApplication<TBuilder>(TBuilder builder, object concreteApplication) where TBuilder : IApplicationBuilderAbstraction => throw new NotImplementedException();
    //    public Abstractions.Adapters.IConcreteApplicationAdapter CreateConcreteApplicationAdapter(object concreteApplication) => throw new NotImplementedException();
    //    public IConcreteApplicationAdapter CreateConcreteApplicationAdapter() => throw new NotImplementedException();
    //    public Abstractions.Adapters.IConcreteBuilderAdapter CreateConcreteBuilderAdapter() => throw new NotImplementedException();
    //}
}
