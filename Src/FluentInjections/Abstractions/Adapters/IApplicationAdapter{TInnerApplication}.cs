// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections.Abstractions.Adapters
{
    public interface IApplicationAdapter<TInnerApplication> : IApplicationAdapter
    {
        /// <summary>
        /// Provides access to the underlying application instance.
        /// </summary>
        TInnerApplication Application { get; }
    }
}
