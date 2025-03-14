// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Core.Abstractions;

using Microsoft.CodeAnalysis;

using System.Reflection;

namespace FluentInjections.Core.Filters
{
    public class PredicateAssemblyFilter : IAssemblyFilter
    {
        private readonly Func<Assembly, bool> _predicate;

        public PredicateAssemblyFilter(Func<Assembly, bool> predicate)
        {
            _predicate = predicate;
        }

        public bool ShouldInclude(Assembly assembly) => _predicate(assembly);
        public bool ShouldInclude(AssemblyName assemblyName) => true;
        public bool ShouldInclude(AssemblyMetadata assemblyMetadata) => true;
    }
}
