// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.CodeAnalysis;

using System.Reflection;

namespace FluentInjections.Core.Abstractions
{
    public interface IAssemblyFilter
    {
        bool ShouldInclude(Assembly assembly);
        bool ShouldInclude(AssemblyName assemblyName);
        bool ShouldInclude(AssemblyMetadata assemblyMetadata);
    }
}
