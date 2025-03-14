// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Core.Abstractions;
using FluentInjections.Core.Caching;

using Microsoft.CodeAnalysis;

using System.Reflection;
namespace FluentInjections.Tests.Units.Core.Caching
{
    public class TypeDiscoveryCacheTests
    {
        [Fact]
        public async Task GetOrAddAsync_ShouldReturnCachedValue_WhenKeyExists()
        {
            // Arrange
            var cache = new TypeDiscoveryCache();
            var key = new CacheKey { AssemblyFilter = new TestAssemblyFilter(), InterfaceType = typeof(object), AttributeType = typeof(object) };
            var expectedTypes = new List<Type> { typeof(string) };
            await cache.GetOrAddAsync(key, _ => Task.FromResult(expectedTypes));

            // Act
            var result = await cache.GetOrAddAsync(key, _ => Task.FromResult(new List<Type>()));

            // Assert
            Assert.Equal(expectedTypes, result);
        }

        [Fact]
        public async Task GetOrAddAsync_ShouldAddValue_WhenKeyDoesNotExist()
        {
            // Arrange
            var cache = new TypeDiscoveryCache();
            var key = new CacheKey { AssemblyFilter = new TestAssemblyFilter(), InterfaceType = typeof(object), AttributeType = typeof(object) };
            var expectedTypes = new List<Type> { typeof(string) };

            // Act
            var result = await cache.GetOrAddAsync(key, _ => Task.FromResult(expectedTypes));

            // Assert
            Assert.Equal(expectedTypes, result);
        }

        private class TestAssemblyFilter : IAssemblyFilter
        {
            public bool ShouldInclude(Assembly assembly) => true;
            public bool ShouldInclude(AssemblyName assemblyName) => true;
            public bool ShouldInclude(AssemblyMetadata assemblyMetadata) => true;
        }
    }
}
