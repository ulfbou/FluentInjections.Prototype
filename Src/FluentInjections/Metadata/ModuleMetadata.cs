// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections.Metadata
{
    public record ModuleMetadata(
        Type ModuleType,
        Type ConfiguratorType,
        Type ComponentType,
        int Priority = 0,
        IEnumerable<Type> Dependencies = null!);
}
