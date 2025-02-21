// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using System.Diagnostics;
namespace FluentInjections;

public static class InjectionBuilder
{
    public static IInjectionBuilder<TBuilder> For<TBuilder>(string[]? arguments = null, IServiceCollection? services = null)
        where TBuilder : class, IApplicationBuilder<TBuilder>
    {
        var composer = new InternalServicesComposer<TBuilder>(arguments, services);
        var factory = composer.CreateFactory(arguments);
        return factory.CreateBuilder();
    }
}
