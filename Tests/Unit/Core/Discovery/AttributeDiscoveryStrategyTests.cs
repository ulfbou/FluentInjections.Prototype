// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentAssertions;

using Microsoft.Extensions.Logging;

using Moq;

using System.Reflection;

namespace FluentInjections.Tests.Units.Core.Discovery
{
    public class AttributeDiscoveryStrategyTests : IClassFixture<AttributeDiscoveryStrategyFixture>
    {
        [Fact]
        public async Task Discover_ShouldLogError_WhenReflectionTypeLoadExceptionOccurs()
        {
            // Arrange
            var fixture = new AttributeDiscoveryStrategyFixture();
            fixture.MockLogger.Setup(l => l.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()
            )).Verifiable();

            // Act
            await fixture.Strategy.Discover(fixture.MockContext.Object, CancellationToken.None).ToListAsync();

            // Assert
            fixture.VerifyReflectionTypeLoadExceptionLogged(fixture.MockAssemblyWithException.Object);
            fixture.MockLogger.Verify();
        }

        [Fact]
        public async Task Discover_SimpleLogTest()
        {
            // Arrange
            var fixture = new AttributeDiscoveryStrategyFixture();
            fixture.MockLogger.Setup(l => l.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()
            )).Verifiable();
            var strategy = new LogTestAttributeDiscoveryStrategy(fixture.MockLoggerFactory.Object);
            fixture.SetupContext(new[] { Assembly.GetExecutingAssembly() }, null!, typeof(TestAttribute));

            // Act
            await foreach (var _ in strategy.Discover(fixture.MockContext.Object)) { }

            // Assert
            fixture.MockLogger.Verify(l => l.Log(LogLevel.Debug, It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(), null, (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()), Times.Once);
            fixture.MockLogger.Verify();

            //Cleanup
            fixture.ResetContext();
        }

        [Fact]
        public async Task Discover_ShouldReturnTypesWithSpecifiedAttribute()
        {
            // Arrange
            var fixture = new AttributeDiscoveryStrategyFixture();
            fixture.SetupContext(new[] { Assembly.GetExecutingAssembly() }, null!, typeof(TestAttribute));

            // Act
            var types = await fixture.Strategy.Discover(fixture.MockContext.Object).ToListAsync();

            // Assert
            types.Should().Contain(typeof(TestClassWithAttribute));
            types.Should().NotContain(typeof(TestClassWithoutAttribute));

            //Cleanup
            fixture.ResetContext();
        }

        [Fact]
        public async Task Discover_ShouldReturnTypesImplementingSpecifiedInterface()
        {
            // Arrange
            var fixture = new AttributeDiscoveryStrategyFixture();
            fixture.SetupContext(new[] { Assembly.GetExecutingAssembly() }, typeof(ITestInterface), null!);

            // Act
            var types = await fixture.Strategy.Discover(fixture.MockContext.Object).ToListAsync();

            // Assert
            types.Should().Contain(typeof(TestClassWithInterface));
            types.Should().NotContain(typeof(TestClassWithoutInterface));

            //Cleanup
            fixture.ResetContext();
        }

        [Fact]
        public async Task Discover_ShouldReturnEmpty_WhenNoMatchingTypes()
        {
            // Arrange
            var fixture = new AttributeDiscoveryStrategyFixture();
            fixture.SetupContext(new[] { Assembly.GetExecutingAssembly() }, typeof(IDisposable), typeof(TestAttribute));

            // Act
            var types = await fixture.Strategy.Discover(fixture.MockContext.Object).ToListAsync();

            // Assert
            types.Should().BeEmpty();

            //Cleanup
            fixture.ResetContext();
        }

        [Fact]
        public async Task Discover_ShouldHandleEmptyAssemblies()
        {
            // Arrange
            var fixture = new AttributeDiscoveryStrategyFixture();
            fixture.SetupContext(new Assembly[0], typeof(ITestInterface), typeof(TestAttribute));

            // Act
            var types = await fixture.Strategy.Discover(fixture.MockContext.Object).ToListAsync();

            // Assert
            types.Should().BeEmpty();

            //Cleanup
            fixture.ResetContext();
        }
    }
}
