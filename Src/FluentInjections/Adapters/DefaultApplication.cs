// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions;
using FluentInjections.Abstractions.Adapters;

namespace FluentInjections
{
    internal sealed class DefaultApplication<TBuilder> : IApplication<TBuilder>
            where TBuilder : IAppBuilderAbstraction
    {
        private readonly TBuilder _builder;
        private readonly IConcreteApplicationAdapter<object> _applicationAdapter; // Using object for now
        private readonly object _concreteApplication; // Using object for now

        public DefaultApplication(
            TBuilder builder,
            IConcreteApplicationAdapter<object> applicationAdapter, // Using object for now
            object concreteApplication)
        {
            _builder = builder;
            _applicationAdapter = applicationAdapter;
            _concreteApplication = concreteApplication;
        }

        public TBuilder Builder => _builder;

        public Task RunAsync(CancellationToken cancellationToken = default) =>
            _applicationAdapter.RunApplicationAsync(_concreteApplication, cancellationToken);

        public Task StartAsync(CancellationToken cancellationToken = default) =>
            _applicationAdapter.StartApplicationAsync(_concreteApplication, cancellationToken);

        public Task StopAsync(CancellationToken cancellationToken = default) =>
            _applicationAdapter.StopApplicationAsync(_concreteApplication, cancellationToken);
    }
}
