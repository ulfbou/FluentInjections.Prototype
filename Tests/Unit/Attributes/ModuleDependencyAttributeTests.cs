// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentAssertions;

using FluentInjections.Attributes;

namespace FluentInjections.Tests.Units.Attributes
{
    public class ModuleDependencyAttributeTests
    {
        [Fact]
        public void Constructor_ShouldSetDependencyType()
        {
            // Arrange
            var dependencyType = typeof(string);

            // Act
            var attribute = new ModuleDependencyAttribute(dependencyType);

            // Assert
            attribute.DependencyType.Should().Be(dependencyType);
        }
    }
}
