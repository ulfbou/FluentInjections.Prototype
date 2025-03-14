// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Core.Abstractions;

using System.Reflection;

namespace FluentInjections.Core.Discovery
{
    public interface ITypeDiscoveryContext
    {
        IEnumerable<Assembly> Assemblies { get; set; }
        IAssemblyFilter AssemblyFilter { get; set; }
        Type AttributeType { get; set; }
        Type InterfaceType { get; set; }
    }
}