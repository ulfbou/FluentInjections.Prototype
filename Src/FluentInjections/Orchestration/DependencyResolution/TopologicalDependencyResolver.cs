// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Metadata;

namespace FluentInjections.Orchestration.DependencyResolution
{
    public class TopologicalDependencyResolver : IDependencyResolver
    {
        public List<ModuleMetadata> ResolveDependencies(List<ModuleMetadata> modules)
        {
            var sortedModules = new List<ModuleMetadata>();
            var visited = new HashSet<Type>();
            var tempVisited = new HashSet<Type>();

            foreach (var module in modules)
            {
                Visit(module, modules, visited, tempVisited, sortedModules);
            }

            return sortedModules;
        }

        private void Visit(ModuleMetadata module, List<ModuleMetadata> modules, HashSet<Type> visited, HashSet<Type> tempVisited, List<ModuleMetadata> sortedModules)
        {
            if (tempVisited.Contains(module.ModuleType))
            {
                throw new InvalidOperationException($"Circular dependency detected: {module.ModuleType.FullName}");
            }

            if (visited.Contains(module.ModuleType))
            {
                return;
            }

            tempVisited.Add(module.ModuleType);

            if (module.Dependencies != null)
            {
                foreach (var dependencyType in module.Dependencies)
                {
                    var dependencyModule = modules.FirstOrDefault(m => m.ModuleType == dependencyType);
                    if (dependencyModule != null)
                    {
                        Visit(dependencyModule, modules, visited, tempVisited, sortedModules);
                    }
                }
            }

            visited.Add(module.ModuleType);
            tempVisited.Remove(module.ModuleType);
            sortedModules.Add(module);
        }
    }
}
