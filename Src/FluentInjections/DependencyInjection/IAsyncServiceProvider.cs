﻿// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.


namespace FluentInjections.DependencyInjection
{
    public interface IAsyncServiceProvider : IAsyncDisposable
    {
        ValueTask<object?> GetServiceAsync(Type serviceType, CancellationToken cancellationToken = default);
    }
}
