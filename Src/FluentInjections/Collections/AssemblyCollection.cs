// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Validation;

using Microsoft.Extensions.Logging;

using System.Collections;
using System.Reflection;

namespace FluentInjections.Collections;

public class AssemblyCollection : IEnumerable<Assembly>
{
    private readonly ILogger<AssemblyCollection> _logger;

    public List<Assembly> Assemblies { get; }

    public AssemblyCollection(ILogger<AssemblyCollection> logger)
    {
        Guard.NotNull(logger, nameof(logger));
        _logger = logger;
        Assemblies = new List<Assembly>();
    }

    public void Add(Assembly assembly)
    {
        Guard.NotNull(assembly, nameof(assembly));
        _logger.LogDebug("Adding assembly {AssemblyName}", assembly.FullName);
        Assemblies.Add(assembly);
    }

    public void AddRange(IEnumerable<Assembly> assemblies)
    {
        Guard.NotNull(assemblies, nameof(assemblies));
        Assemblies.AddRange(assemblies);
        _logger.LogDebug("Adding {Count} assemblies", assemblies.Count());
    }

    internal void Remove(Assembly assembly)
    {
        Guard.NotNull(assembly, nameof(assembly));
        Assemblies.Remove(assembly);
        _logger.LogDebug("Removing assembly {AssemblyName}", assembly.FullName);
    }

    internal void RemoveRange(IEnumerable<Assembly> assemblies)
    {
        Guard.NotNull(assemblies, nameof(assemblies));
        assemblies.ToList().ForEach(a => Assemblies.Remove(a));
        _logger.LogDebug("Removing {Count} assemblies", assemblies.Count());
    }

    public IEnumerator<Assembly> GetEnumerator() => ((IEnumerable<Assembly>)Assemblies).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Assemblies).GetEnumerator();
}
