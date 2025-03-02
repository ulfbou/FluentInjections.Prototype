// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Adapters.AspNetCore;

using Microsoft.AspNetCore.Builder;

namespace FluentInjections.Abstractions.AspNetCore
{
    ///// <summary>
    ///// Concrete implementation of <see cref="IApplicationBuilderAbstraction"/> for ASP.NET Core Web Applications.
    ///// </summary>
    //public class AspNetCoreAppBuilderAbstraction : IApplicationBuilderAbstraction
    //{
    //    /// <inheritdoc />
    //    public string FrameworkIdentifier => "AspNetCore";

    //    /// <inheritdoc />
    //    public Type AdapterFactoryType => typeof(AspNetCoreAdapterFactory);

    //    /// <inheritdoc />
    //    public Type ConcreteApplicationType => typeof(WebApplication);

    //    public Task<IApplicationAbstraction> BuildAsync(CancellationToken cancellationToken = default) => throw new NotImplementedException();
    //}
}
