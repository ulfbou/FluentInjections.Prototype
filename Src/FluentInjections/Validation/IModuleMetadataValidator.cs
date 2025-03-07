// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Metadata;

namespace FluentInjections.Validation
{
    public interface IModuleMetadataValidator
    {
        void Validate(ModuleMetadata metadata);
    }
}
