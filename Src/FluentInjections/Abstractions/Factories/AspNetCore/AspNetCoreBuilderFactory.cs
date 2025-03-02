// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions.AspNetCore;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;

namespace FluentInjections.Abstractions.Factories.AspNetCore
{
    ///// <summary>
    ///// Concrete implementation of <see cref="IBuilderFactory{TBuilder}"/> for ASP.NET Core Web Applications.
    ///// </summary>
    //public class AspNetCoreBuilderFactory : IBuilderFactory<AspNetCoreAppBuilderAbstraction>
    //{
    //    /// <inheritdoc />
    //    public AspNetCoreAppBuilderAbstraction CreateBuilder(string[]? args = null, ILoggerFactory? loggerFactory = null)
    //    {
    //        var builder = WebApplication.CreateBuilder(args ?? Array.Empty<string>());

    //        if (loggerFactory != null)
    //        {
    //            builder.Logging.ClearProviders();
    //        }
    //        return new AspNetCoreAppBuilderAbstraction();
    //    }

    //}
}
