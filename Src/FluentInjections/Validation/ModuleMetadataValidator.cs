// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Metadata;

namespace FluentInjections.Validation
{
    public class ModuleMetadataValidator : IModuleMetadataValidator
    {
        public void Validate(ModuleMetadata metadata)
        {
            Guard.NotNull(metadata, nameof(metadata));
            Guard.NotNull(metadata.ModuleType, nameof(metadata.ModuleType));
            Guard.NotNull(metadata.ConfiguratorType, nameof(metadata.ConfiguratorType));
            Guard.NotNull(metadata.ComponentType, nameof(metadata.ComponentType));
        }
    }
}