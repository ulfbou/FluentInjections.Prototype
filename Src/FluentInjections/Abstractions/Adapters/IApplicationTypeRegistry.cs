// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections.Abstractions.Adapters
{
    public interface IApplicationTypeRegistry
    {
        /// <summary>
        /// Gets the registered application types.
        /// </summary>
        IReadOnlyCollection<IApplicationTypeMetadata> RegisteredApplications { get; }

        /// <summary>
        /// Registers an application type with its corresponding builder.
        /// </summary>
        void Register<TAdapter, TInnerApplication, TBuilder>(Func<TBuilder> factory)
            where TAdapter : IApplicationAdapter<TInnerApplication>
            where TBuilder : IApplicationBuilderAdapter<TInnerApplication>;
    }
}
