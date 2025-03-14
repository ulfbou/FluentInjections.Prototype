// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentAssertions;

using FluentInjections.Core.Discovery.Metadata;

namespace FluentInjections.Tests.Units.Core.Discovery.Metadata
{
    public class AdapterMetadataAttributeTests
    {
        [Fact]
        public void Constructor_ShouldSetProperties()
        {
            // Arrange
            var adapterType = typeof(string);
            var frameworkIdentifier = "TestFramework";
            var frameworkVersion = "2.0";

            // Act
            var attribute = new AdapterMetadataAttribute(adapterType, frameworkIdentifier, frameworkVersion);

            // Assert
            attribute.AdapterType.Should().Be(adapterType);
            attribute.FrameworkIdentifier.Should().Be(frameworkIdentifier);
            attribute.FrameworkVersion.Should().Be(frameworkVersion);
        }

        [Fact]
        public void Constructor_ShouldUseDefaultValues()
        {
            // Arrange
            var adapterType = typeof(int);

            // Act
            var attribute = new AdapterMetadataAttribute(adapterType);

            // Assert
            attribute.AdapterType.Should().Be(adapterType);
            attribute.FrameworkIdentifier.Should().Be("AspNetCore");
            attribute.FrameworkVersion.Should().Be("1.0");
        }

        [Fact]
        public void Constructor_ShouldThrowExceptionOnNullAdapterType()
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentNullException>(() => new AdapterMetadataAttribute(null!));
        }

        [Fact]
        public void Constructor_ShouldThrowExceptionOnNullFrameworkIdentifier()
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentException>(() => new AdapterMetadataAttribute(typeof(string), null!, "1.0"));
        }

        [Fact]
        public void Constructor_ShouldThrowExceptionOnNullFrameworkVersion()
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentException>(() => new AdapterMetadataAttribute(typeof(string), "AspNetCore", null!));
        }
    }
}
