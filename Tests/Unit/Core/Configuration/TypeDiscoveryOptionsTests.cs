// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentAssertions;

using FluentInjections.Abstractions;
using FluentInjections.Core.Configuration;
using FluentInjections.Core.Discovery;
using FluentInjections.Core.Discovery.Metadata;

using Microsoft.AspNetCore.Builder;

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
            options.AssemblyFilterPredicate.Should().Be(nameof(FluentInjections));
            options.InterfaceTypeName.Should().Be(nameof(IFluentOrchestrator<WebApplication>));
            options.AttributeTypeName.Should().Be(nameof(AdapterMetadataAttribute));
        }

        [Fact]
        public void Properties_ShouldSetAndGetValues()
        {
            // Arrange
            var options = new TypeDiscoveryOptions
            {
                AssemblyFilterPredicate = nameof(FluentInjections.Tests.Units),
                InterfaceTypeName = nameof(ITypeDiscoveryContext),
                AttributeTypeName = nameof(AdapterMetadataAttribute)
            };

            // Assert
            options.AssemblyFilterPredicate.Should().Be(nameof(FluentInjections.Tests.Units));
            options.InterfaceTypeName.Should().Be(nameof(ITypeDiscoveryContext));
            options.AttributeTypeName.Should().Be(nameof(AdapterMetadataAttribute));
        }
    }
}
