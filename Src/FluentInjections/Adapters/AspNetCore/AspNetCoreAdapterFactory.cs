// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions;
using FluentInjections.Abstractions.Adapters;
using FluentInjections.Abstractions.AspNetCore;
using FluentInjections.Abstractions.Factories;
using FluentInjections.Abstractions.Factories.AspNetCore;

using Microsoft.AspNetCore.Builder;

namespace FluentInjections.Adapters.AspNetCore
{
    ///// <summary>
    ///// Concrete implementation of <see cref="IApplicationAdapterFactory"/> for ASP.NET Core Web Applications.
    ///// </summary>
    //public class AspNetCoreAdapterFactory : IApplicationAdapterFactory
    //{
    //    /// <inheritdoc />
    //    public IConcreteBuilderAdapter CreateConcreteBuilderAdapter()
    //    {
    //        return new AspNetCoreBuilderAdapter();
    //    }

    //    /// <inheritdoc />
    //    public IConcreteApplicationAdapter CreateConcreteApplicationAdapter() => throw new NotImplementedException();
    //    public IApplication<TBuilder> CreateApplication<TBuilder>(TBuilder builder, object concreteApplication) where TBuilder : Abstractions.IApplicationBuilderAbstraction => throw new NotImplementedException();
    //    public IConcreteApplicationAdapter CreateConcreteApplicationAdapter(object concreteApplication)
    //    {
    //        if (!(concreteApplication is WebApplication webApplication))
    //        {
    //            throw new ArgumentException($"Expected concrete application of type {typeof(WebApplication).FullName} but got {concreteApplication?.GetType().FullName ?? "null"}.", nameof(concreteApplication));
    //        }
    //        return new AspNetCoreApplicationAdapter(webApplication);
    //    }
    //}
}