// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions.Adapters;

namespace FluentInjections.Tests.Units.Orchestration
{
    public class TestApplicationAdapter : IApplicationAdapter<TestApplication>
    {
        public TestApplication Application => new TestApplication();

        public ValueTask DisposeAsync() => ValueTask.CompletedTask;
        public Task RunAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
        public Task StartAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
        public Task StopAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
    }
}
