// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions.Adapters;
using FluentInjections.Core.Configuration;

using Moq;
using FluentAssertions;

namespace FluentInjections.Tests.Units.Orchestration
{
    public class FluentOrchestratorWithDiscoveryOptionsTestsForTestApplication : FluentOrchestratorWithDiscoveryOptionsTests<TestApplication> { }
    public abstract class FluentOrchestratorWithDiscoveryOptionsTests<TApplication>
    {
        [Fact]
        public void Constructor_ShouldInitializeWithDefaultValues()
        {
            // Arrange
            var fixture = new FluentOrchestratorFixture<TApplication>();

            // Act
            var orchestrator = fixture.CreateOrchestrator();

            // Assert
            orchestrator.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_ShouldInitializeWithProvidedValues()
        {
            // Arrange
            var fixture = new FluentOrchestratorFixture<TApplication>();
            string[] arguments = { "--test", "value" };

            // Act
            var orchestrator = fixture.CreateOrchestrator(arguments);

            // Assert
            orchestrator.Should().NotBeNull();
        }
    }
}
