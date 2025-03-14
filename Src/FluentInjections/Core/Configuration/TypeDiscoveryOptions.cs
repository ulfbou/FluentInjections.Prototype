// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions;
using FluentInjections.Core.Discovery.Metadata;

using Microsoft.AspNetCore.Builder;

namespace FluentInjections.Core.Configuration
{
    public class TypeDiscoveryOptions
    {
        public static TypeDiscoveryOptions DefaultOptions = new TypeDiscoveryOptions
        {
            AssemblyFilterPredicate = nameof(FluentInjections),
            InterfaceTypeName = nameof(IFluentOrchestrator<WebApplication>),
            AttributeTypeName = nameof(AdapterMetadataAttribute)
        };

        public required string AssemblyFilterPredicate { get; set; }
        public required string InterfaceTypeName { get; set; }
        public required string AttributeTypeName { get; set; }
    }
}
