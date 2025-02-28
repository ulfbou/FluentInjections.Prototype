// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.DependencyInjection;

namespace FluentInjections.Abstractions
{
    public interface IInjectionBuilder<TBuilder>
        where TBuilder : IAppBuilderAbstraction
    {
        Task<IApplication<TBuilder>> BuildAsync(CancellationToken cancellationToken = default);
    }
}
