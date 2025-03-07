// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections.Internal.Discovery.Configuration
{
    public class ModuleDiscoveryOptions
    {
        public List<string> AssemblyFilters { get; set; } = new List<string>();
        public List<string> TypeFilters { get; set; } = new List<string>();
    }
}