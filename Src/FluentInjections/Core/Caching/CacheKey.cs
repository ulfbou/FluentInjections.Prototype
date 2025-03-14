// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Core.Abstractions;

namespace FluentInjections.Core.Caching
{
    public class CacheKey
    {
        public required IAssemblyFilter AssemblyFilter { get; set; }
        public required Type InterfaceType { get; set; }
        public required Type AttributeType { get; set; }

        public override int GetHashCode()
        {
            return HashCode.Combine(AssemblyFilter, InterfaceType, AttributeType);
        }

        public override bool Equals(object? obj)
        {
            return obj is CacheKey other && other.AssemblyFilter == AssemblyFilter && other.InterfaceType == InterfaceType && other.AttributeType == AttributeType;
        }
    }
}
