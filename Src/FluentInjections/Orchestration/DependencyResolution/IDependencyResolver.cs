// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections.Orchestration.DependencyResolution
{
    public interface IDependencyResolver
    {
        List<Metadata.ModuleMetadata> ResolveDependencies(List<Metadata.ModuleMetadata> modules);
    }
}
