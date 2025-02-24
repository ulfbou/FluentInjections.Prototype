// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Application;
using FluentInjections.Injection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System.Reflection;

namespace FluentInjections.Application;

public interface IApplicationBuilder<TBuilder> : IDisposable
        where TBuilder : IApplicationBuilder<TBuilder>
{
    IServiceCollection Services { get; }
    TBuilder Builder { get; }
    TInnerBuilder GetInnerBuilder<TInnerBuilder>();
    TBuilder AddConfigurationSource(IConfigurationSource configurationSource);

    /// <summary>
    /// Builds the application asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="IApplication{TBuilder}"/> instance.</returns>
    Task<IApplication<TBuilder>> BuildAsync(CancellationToken cancellationToken = default);
}
