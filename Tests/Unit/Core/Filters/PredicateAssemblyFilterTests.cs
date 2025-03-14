// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Xunit;
using FluentAssertions;
using System.Reflection;
using FluentInjections.Core.Filters;
using System.IO;
using Microsoft.CodeAnalysis;

namespace FluentInjections.Tests.Units.Core.Filters
{
    public class PredicateAssemblyFilterTests
    {
        [Fact]
        public void ShouldInclude_ShouldReturnTrue_WhenPredicateTrue()
        {
            // Arrange
            var filter = new PredicateAssemblyFilter(a => a.FullName == Assembly.GetExecutingAssembly().FullName);

            // Act
            var result = filter.ShouldInclude(Assembly.GetExecutingAssembly());

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void ShouldInclude_ShouldReturnFalse_WhenPredicateFalse()
        {
            // Arrange
            var filter = new PredicateAssemblyFilter(a => a.FullName == "NonExistentAssembly");

            // Act
            var result = filter.ShouldInclude(Assembly.GetExecutingAssembly());

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void ShouldIncludeAssemblyName_ShouldReturnTrue()
        {
            // Arrange
            var filter = new PredicateAssemblyFilter(a => true);
            var assemblyName = Assembly.GetExecutingAssembly().GetName();

            // Act
            var result = filter.ShouldInclude(assemblyName);

            // Assert
            result.Should().BeTrue();
        }
    }
}
