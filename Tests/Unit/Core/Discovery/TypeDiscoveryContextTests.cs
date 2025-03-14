// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentAssertions;
using System.Reflection;
using FluentInjections.Core.Filters;
using FluentInjections.Core.Discovery;

namespace FluentInjections.Tests.Units.Core.Discovery
{
    public class TypeDiscoveryContextTests
    {
        [Fact]
        public void Properties_ShouldSetAndGetValues()
        {
            // Arrange
            var filter = new PredicateAssemblyFilter(a => true);
            var interfaceType = typeof(string);
            var attributeType = typeof(int);
            var assemblies = new[] { Assembly.GetExecutingAssembly() };

            // Act
            var context = new TypeDiscoveryContext
            {
                AssemblyFilter = filter,
                InterfaceType = interfaceType,
                AttributeType = attributeType,
                Assemblies = assemblies
            };

            // Assert
            context.AssemblyFilter.Should().Be(filter);
            context.InterfaceType.Should().Be(interfaceType);
            context.AttributeType.Should().Be(attributeType);
            context.Assemblies.Should().BeEquivalentTo(assemblies);
        }
    }
}
