// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentAssertions;

using FluentInjections.Core.Configuration;

namespace FluentInjections.Tests.Units.Core.Configuration
{
    public class TypeDiscoveryOptionsTests
    {
        [Fact]
        public void DefaultOptions_ShouldHaveDefaultValues()
        {
            // Arrange, Act
            var options = TypeDiscoveryOptions.DefaultOptions;

            // Assert
            options.AssemblyFilterPredicate.Should().Be("FluentInjections");
            options.InterfaceTypeName.Should().Be("IFluentOrchestrator");
            options.AttributeTypeName.Should().Be("FluentInjections");
        }

        [Fact]
        public void Properties_ShouldSetAndGetValues()
        {
            // Arrange
            var options = new TypeDiscoveryOptions
            {
                AssemblyFilterPredicate = "Test",
                InterfaceTypeName = "ITest",
                AttributeTypeName = "TestAttribute"
            };

            // Assert
            options.AssemblyFilterPredicate.Should().Be("Test");
            options.InterfaceTypeName.Should().Be("ITest");
            options.AttributeTypeName.Should().Be("TestAttribute");
        }
    }
}
