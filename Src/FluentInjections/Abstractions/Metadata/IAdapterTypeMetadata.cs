// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections.Abstractions.Metadata
{
    /// <summary>
    /// Represents metadata for an adapter type.
    /// </summary>
    public interface IAdapterTypeMetadata
    {
        /// <summary>
        /// Gets the type of the adapter.
        /// </summary>
        Type AdapterType { get; }

        /// <summary>
        /// Gets the framework identifier for the adapter.
        /// </summary>
        string FrameworkIdentifier { get; }

        /// <summary>
        /// Gets the version of the framework that the adapter targets.
        /// </summary>
        string FrameworkVersion { get; }

        // Add other metadata properties as needed
    }
}
