// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Collections;

using Microsoft.Extensions.DependencyInjection;

namespace FluentInjections;

// ApplicationConfigurationManager (No longer a singleton)
public interface IApplicationConfigurator
{
    AssemblyCollection ServiceAssemblies { get; }
    AssemblyCollection MiddlewareAssemblies { get; }
    AssemblyCollection LifecycleAssemblies { get; }

    void Configure(IServiceCollection services);
}
