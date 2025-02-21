// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.



namespace FluentInjections;

public interface IApplication<TBuilder>
    where TBuilder : IApplicationBuilder<TBuilder>
{
    TBuilder Builder { get; }

    Task RunAsync(CancellationToken? cancellationToken = default);
    Task StopAsync(CancellationToken? cancellationToken = default);
    Task StartAsync(CancellationToken? cancellationToken = default);
    Task<TInnerApplication> GetInnerApplicationAsync<TInnerApplication>(CancellationToken? cancellationToken = null);
}
