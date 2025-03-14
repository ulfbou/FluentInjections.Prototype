// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentAssertions;

using FluentInjections.Core.Abstractions;
using FluentInjections.Core.Configuration;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Moq;

namespace FluentInjections.Tests.Units.Orchestration
{
    public class FluentOrchestratorBuildAsyncTestsForTestApplication : FluentOrchestratorBuildAsyncTests<TestApplication> { }

    public abstract class FluentOrchestratorBuildAsyncTests<TApplication>
    {
        [Fact]
        public async Task BuildAsync_ShouldReturnApplication()
        {
            var fixture = new FluentOrchestratorFixture<TestApplication>();
            fixture.SetupAdapter();
            fixture.SetupApplicationBuilderAdapter(Activator.CreateInstance<TestApplication>());

            var app = await fixture.Orchestrator.BuildAsync();

            app.Should().NotBeNull();
        }

        [Fact]
        public async Task BuildAsync_ShouldThrowException_WhenNoAdapterRegistered()
        {
            var fixture = new FluentOrchestratorFixture<TestApplication>();

            await Assert.ThrowsAsync<InvalidOperationException>(() => fixture.Orchestrator.BuildAsync());
        }

        [Fact]
        public async Task BuildAsync_ShouldUseRegisteredAdapter()
        {
            var fixture = new FluentOrchestratorFixture<TestApplication>();
            fixture.SetupAdapter();
            fixture.SetupApplicationBuilderAdapter(Activator.CreateInstance<TestApplication>());

            await fixture.Orchestrator.BuildAsync();

            fixture.MockApplicationBuilderAdapter.Verify(b => b.BuildAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task BuildAsync_ShouldConfigureServices()
        {
            var fixture = new FluentOrchestratorFixture<TestApplication>();
            fixture.SetupConfiguration(nameof(FluentInjections), new Mock<IConfigurationSection>());
            fixture.SetupAdapter();
            fixture.SetupApplicationBuilderAdapter(Activator.CreateInstance<TestApplication>());
            fixture.RegisterMockAdapterIntoServices(fixture.MockApplicationBuilderAdapter.Object);

            await fixture.Orchestrator.BuildAsync();

            fixture.MockApplicationBuilderAdapter.Verify(
                b => b.ConfigureServices(It.IsAny<IServiceCollection>())
                , Times.Once);
        }
    }
}
