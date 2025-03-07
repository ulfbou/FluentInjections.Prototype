// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions.Adapters;

namespace FluentInjections.Abstractions
{
    public interface IFluentOrchestrator<TApplication>
    {
        Task<IApplicationAdapter<TApplication>> BuildAsync(CancellationToken? ct = null);
        IFluentOrchestrator<TApplication> UseAdapter<TAdapter>() where TAdapter : IApplicationAdapter<TApplication>;
    }
}
