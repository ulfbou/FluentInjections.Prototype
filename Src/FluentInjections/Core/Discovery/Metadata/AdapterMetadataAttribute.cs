// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions.Metadata;
using FluentInjections.Validation;

namespace FluentInjections.Core.Discovery.Metadata
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class AdapterMetadataAttribute : Attribute, IAdapterTypeMetadata
    {
        public Type AdapterType { get; }
        public string FrameworkIdentifier { get; }
        public string FrameworkVersion { get; }

        public AdapterMetadataAttribute(Type adapterType, string frameworkIdentifier = "AspNetCore", string frameworkVersion = "1.0")
        {
            Guard.NotNull(adapterType, nameof(adapterType));
            Guard.NotNullOrWhiteSpace(frameworkIdentifier, nameof(frameworkIdentifier));
            Guard.NotNullOrWhiteSpace(frameworkVersion, nameof(frameworkVersion));
            AdapterType = adapterType;
            FrameworkIdentifier = frameworkIdentifier;
            FrameworkVersion = frameworkVersion;
        }
    }
}
