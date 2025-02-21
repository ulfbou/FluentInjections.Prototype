// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.DependencyInjection;

using System.Reflection;

using static System.Net.Mime.MediaTypeNames;

namespace FluentInjections;

public interface IApplicationBuilder<TBuilder> : IDisposable
    where TBuilder : IApplicationBuilder<TBuilder>
{
    IServiceCollection Services { get; }
    TBuilder Builder { get; }
    Task<IApplication<TBuilder>> BuildAsync(CancellationToken? cancellationToken = default);
    TInnerBuilder GetInnerBuilder<TInnerBuilder>();
}
