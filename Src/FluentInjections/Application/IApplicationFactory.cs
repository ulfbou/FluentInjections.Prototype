// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections.Application;

public interface IApplicationFactory<TBuilder>
    where TBuilder : IApplicationBuilder<TBuilder>
{
    Task<IApplication<TBuilder>> CreateApplicationAsync(TBuilder builder);
}
