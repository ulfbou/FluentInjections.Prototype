// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions.Adapters;

namespace FluentInjections.Abstractions
{
    public interface IFluentOrchestrator<TApplication>
    {
        Task<TApplication> BuildAsync(CancellationToken cancellationToken = default);
        IFluentOrchestrator<TApplication> UseAdapterInstance(string frameworkIdentifier, object adapterInstance);
        IFluentOrchestrator<TApplication> UseAdapter<TAdapter>(string frameworkIdentifier) where TAdapter : IApplicationAdapter<TApplication>;
    }
}
