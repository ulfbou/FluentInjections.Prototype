// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Adapters.AspNetCore;
using FluentAssertions;
using FluentInjections.Abstractions.Adapters;

using Microsoft.AspNetCore.Builder;

using Xunit;

namespace FluentInjections.Tests.Units.Core.Discovery
{
    public class ApplicationTypeRegistryTests
    {
        [Fact]
        public void Register_ShouldAddMetadataAndFactory()
        {
            // Arrange
            var fixture = new ApplicationTypeRegistryFixture();

            // Act
            fixture.Registry.Register<WebApplicationAdapter, WebApplication, WebApplicationBuilderAdapter>(fixture.CreateWebApplicationBuilder);

            // Assert
            fixture.Registry.RegisteredApplications.Should().HaveCount(1);
            IApplicationTypeMetadata metadata = fixture.Registry.RegisteredApplications.First();
            metadata.ApplicationType.Should().Be(typeof(WebApplication));
            metadata.BuilderType.Should().Be(typeof(WebApplicationBuilderAdapter));
            metadata.FrameworkName.Should().Be(ApplicationTypeRegistry.FrameworkName);
        }
    }
}
