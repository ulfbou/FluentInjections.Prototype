// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Core.Abstractions;

using System.Reflection;
using FluentInjections.Core.Discovery;

namespace FluentInjections.Tests.Utils
{
    public class TestTypeDiscoveryContext : ITypeDiscoveryContext
    {
        public required IAssemblyFilter AssemblyFilter { get; set; }
        public required Type InterfaceType { get; set; }
        public required Type AttributeType { get; set; }
        public required IEnumerable<Assembly> Assemblies { get; set; }
    }
}
