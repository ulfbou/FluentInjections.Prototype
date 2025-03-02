// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions.Adapters;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace FluentInjections.Abstractions.AspNetCore
{
    ///// <summary>
    ///// Concrete implementation of <see cref="IConcreteApplicationAdapter"/> for ASP.NET Core Web Applications.
    ///// </summary>
    //public class AspNetCoreApplicationAdapter : IConcreteApplicationAdapter
    //{
    //    private readonly WebApplication _application;

    //    public AspNetCoreApplicationAdapter(WebApplication application)
    //    {
    //        _application = application ?? throw new ArgumentNullException(nameof(application));
    //    }

    //    /// <inheritdoc />
    //    public Task StartAsync(CancellationToken cancellationToken = default)
    //    {
    //        return _application.StartAsync(cancellationToken);
    //    }

    //    /// <inheritdoc />
    //    public Task StopAsync(CancellationToken cancellationToken = default)
    //    {
    //        return _application.StopAsync(cancellationToken);
    //    }

    //    /// <inheritdoc />
    //    public Task RunAsync(CancellationToken cancellationToken = default)
    //    {
    //        return _application.RunAsync(cancellationToken);
    //    }
    //}
}