// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions;
using FluentInjections.Abstractions.Adapters;

namespace FluentInjections
{
    //internal sealed class DefaultApplication<TBuilder> : IApplication<TBuilder>
    //        where TBuilder : Abstractions.IApplicationBuilderAbstraction
    //{
    //    private readonly TBuilder _builder;
    //    private readonly IConcreteApplicationAdapter _applicationAdapter; // Using object for now
    //    private readonly object _concreteApplication; // Using object for now

    //    public DefaultApplication(
    //        TBuilder builder,
    //        IConcreteApplicationAdapter applicationAdapter, // Using object for now
    //        object concreteApplication)
    //    {
    //        _builder = builder;
    //        _applicationAdapter = applicationAdapter;
    //        _concreteApplication = concreteApplication;
    //    }

    //    public TBuilder Builder => _builder;

    //    public async Task RunAsync(CancellationToken cancellationToken = default) =>
    //        await _applicationAdapter.RunAsync(cancellationToken);

    //    public Task StartAsync(CancellationToken cancellationToken = default) =>
    //        _applicationAdapter.StartAsync(cancellationToken);

    //    public Task StopAsync(CancellationToken cancellationToken = default) =>
    //        _applicationAdapter.StopAsync(cancellationToken);
    //}
}
