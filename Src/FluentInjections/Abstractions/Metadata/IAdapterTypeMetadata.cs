// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections.Abstractions.Metadata
{
    public interface IAdapterTypeMetadata
    {
        Type AdapterType { get; }
        string FrameworkIdentifier { get; }
        string FrameworkVersion { get; }
    }
}
