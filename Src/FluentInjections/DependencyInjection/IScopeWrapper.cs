// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections.DependencyInjection;

public interface IScopeWrapper : IAsyncDisposable, IDisposable
{
    Guid Id { get; }
    IServiceProvider ServiceProvider { get; }
}
