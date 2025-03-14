// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace FluentInjections.Abstractions.Adapters
{
    public interface IApplicationBuilderAdapter<TBuiltApplication>
    {
        /// <summary>
        /// Builds the application asynchronously.
        /// </summary>
        Task<IApplicationAdapter<TBuiltApplication>> BuildAsync(CancellationToken cancellationToken = default);

        // Core functionality: Access the underlying builder (for adapter-specific operations)
        object InnerBuilder { get; }

        // Core functionality: Configure services
        void ConfigureServices(IServiceCollection services);
    }
}
