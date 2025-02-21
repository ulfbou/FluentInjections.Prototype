// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Reflection;

namespace FluentInjections;

public interface IAssemblyManager : IEnumerable<Assembly>
{
    void AddAssembly(Assembly assembly);
    void RemoveAssembly(Assembly assembly); // Simplified - caller can pass a collection
    Task DiscoverModulesAsync(IServiceProvider serviceProvider, CancellationToken? cancellationToken = null); // CancellationToken?
    Task ActivateModulesAsync<TConfigurator>(IServiceProvider serviceProvider, CancellationToken? cancellationToken = null) // CancellationToken?
        where TConfigurator : class, IConfigurator;
}
