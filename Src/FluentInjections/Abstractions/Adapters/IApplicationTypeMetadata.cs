// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections.Abstractions.Adapters
{
    public interface IApplicationTypeMetadata
    {
        Type ApplicationType { get; }
        Type BuilderType { get; }
        string FrameworkName { get; }
    }
}
