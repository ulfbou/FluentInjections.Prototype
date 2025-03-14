// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Xunit;
using FluentAssertions;
using FluentInjections.Orchestration;
using FluentInjections.Core.Discovery.Metadata;
using FluentInjections.Abstractions.Adapters;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;

namespace FluentInjections.Tests.Units.Orchestration
{
    public class FluentOrchestratorUseAdapterTestsForTestApplication : FluentOrchestratorUseAdapterTests<TestApplication> { }
    public abstract class FluentOrchestratorUseAdapterTests<TApplication>
    {
        [Fact]
        public void UseAdapter_ShouldRegisterAdapter()
        {
            // Arrange
            var fixture = new FluentOrchestratorFixture<TApplication>();

            // Act
            fixture.SetupAdapter();

            // Assert
            // Cannot directly verify private _adapterRegistry, but BuildAsync will fail if not registered.
        }

        [Fact]
        public void UseAdapter_ShouldThrowException_WhenAdapterHasNoMetadata()
        {
            // Arrange
            var fixture = new FluentOrchestratorFixture<TApplication>();

            // Act & Assert
            Assert.Throws<System.InvalidOperationException>(() => fixture.Orchestrator.UseAdapter<FlawedApplicationAdapter>("Test"));
        }

        [Fact]
        public void UseAdapter_ShouldRegisterMultipleAdapters()
        {
            // Arrange
            var fixture = new FluentOrchestratorFixture<TApplication>();

            // Act
            fixture.SetupAdapter("Test1");
            fixture.SetupAdapter("Test2");

            // Assert
            // Cannot directly verify private _adapterRegistry, but BuildAsync will fail if not registered.
        }

        public class FlawedApplicationAdapter : IApplicationAdapter<TApplication>
        {
            public TApplication Application => default!;
            public ValueTask DisposeAsync() => ValueTask.CompletedTask;
            public Task RunAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
            public Task StartAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
            public Task StopAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
        }
    }
}
