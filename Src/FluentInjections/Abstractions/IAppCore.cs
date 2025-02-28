// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections.Abstractions
{
    public interface IAppCore
    {
        Task RunAsync(CancellationToken? cancellationToken = null);
        Task StopAsync(CancellationToken? cancellationToken = null);
    }
}