// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentAssertions;

using FluentInjections.Adapters.AspNetCore;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FluentInjections.Tests.Units.Adapters.AspNetCore
{
    public class WebApplicationBuilderAdapterTests
    {
        [Fact]
        public async Task BuildAsync_ShouldReturnApplicationAdapter()
        {
            // Arrange
            var fixture = new WebApplicationBuilderAdapterFixture();

            // Act
            var result = await fixture.Adapter.BuildAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<WebApplicationAdapter>();
        }

        [Fact]
        public void ConfigureServices_ShouldAddServicesToBuilder()
        {
            // Arrange
            var fixture = new WebApplicationBuilderAdapterFixture();
            fixture.AddTestService();

            // Act
            fixture.Adapter.ConfigureServices(fixture.Services);

            // Assert
            var serviceProvider = (fixture.Adapter.InnerBuilder as WebApplicationBuilder)!.Services.BuildServiceProvider();
            var testService = serviceProvider.GetService<WebApplicationBuilderAdapterFixture.ITestService>();
            testService.Should().NotBeNull();
            testService.Should().BeOfType<WebApplicationBuilderAdapterFixture.TestService>();
        }

        [Fact]
        public async Task DisposeAsync_ShouldNotThrow_WhenWebApplicationIsDisposable()
        {
            // Arrange
            var fixture = new WebApplicationBuilderAdapterFixture();

            // Act
            await fixture.Adapter.DisposeAsync();

            // Assert
            // No exception should be thrown.
        }

        [Fact]
        public void Constructor_ShouldInitializeLoggingProviders()
        {
            // Arrange
            var fixture = new WebApplicationBuilderAdapterFixture();

            // Assert
            var builderServices = (fixture.Adapter.InnerBuilder as WebApplicationBuilder)!.Services;
            var serviceProvider = builderServices.BuildServiceProvider();
            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            loggerFactory.Should().NotBeNull();
        }

        [Fact]
        public void InnerBuilder_ShouldReturnWebApplicationBuilder()
        {
            // Arrange
            var fixture = new WebApplicationBuilderAdapterFixture();

            // Assert
            fixture.Adapter.InnerBuilder.Should().BeOfType<WebApplicationBuilder>();
        }
    }
}
