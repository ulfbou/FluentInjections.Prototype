// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Validation;

namespace FluentInjections.Discovery
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class AdapterMetadataAttribute : Attribute, Abstractions.Metadata.IAdapterTypeMetadata
    {
        public AdapterMetadataAttribute(Type adapterType, string frameworkIdentifier, string frameworkVersion = "1.0")
        {
            Guard.NotNull(adapterType, nameof(adapterType));
            Guard.NotNullOrWhiteSpace(frameworkIdentifier, nameof(frameworkIdentifier));
            Guard.NotNullOrWhiteSpace(frameworkVersion, nameof(frameworkVersion));
            FrameworkIdentifier = frameworkIdentifier;
            FrameworkVersion = frameworkVersion;
            AdapterType = adapterType;
        }

        public Type AdapterType { get; }
        public string FrameworkIdentifier { get; }
        public string FrameworkVersion { get; }
    }
}