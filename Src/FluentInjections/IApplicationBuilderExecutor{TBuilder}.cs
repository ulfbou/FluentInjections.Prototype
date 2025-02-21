// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections;

public interface IApplicationBuilderExecutor<TBuilder>
    where TBuilder : IApplicationBuilder<TBuilder>
{
    Task<IApplication<TBuilder>> ExecuteBuildAsync(TBuilder builder, CancellationToken? cancellationToken = null);
}
