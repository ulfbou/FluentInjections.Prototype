// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentAssertions;

using FluentInjections.Attributes;
using FluentInjections.Core.Discovery;

using System.Reflection;

using static System.Net.Mime.MediaTypeNames;

namespace FluentInjections.Tests.Units.Orchestration
{
    public class FluentOrchestratorPrivateMethodsTestsForTestApplication : FluentOrchestratorPrivateMethodsTests<TestApplication> { }

    public abstract class FluentOrchestratorPrivateMethodsTests<TApplication>
    {
        [Fact]
        public void CreateDiscoveryContext_ShouldCreateContext()
        {
            var fixture = new FluentOrchestratorFixture<TApplication>();
            var orchestrator = fixture.Orchestrator;
            var method = orchestrator.GetType().GetMethod("CreateDiscoveryContext", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            if (method == null)
            {
                throw new InvalidOperationException("Method CreateDiscoveryContext not found.");
            }

            var context = method.Invoke(orchestrator, null) as TypeDiscoveryContext;

            context.Should().NotBeNull();
        }

        [Fact]
        public void TopologicalSort_ShouldSortModules()
        {
            var fixture = new FluentOrchestratorFixture<TApplication>();
            var orchestrator = fixture.Orchestrator;
            var modules = new List<Type> { typeof(ModuleC), typeof(ModuleA), typeof(ModuleB) };

            var method = orchestrator.GetType().GetMethod("TopologicalSort", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            if (method == null)
            {
                throw new InvalidOperationException("Method TopologicalSort not found.");
            }

            // Invoke TopologicalSort with the correct parameter:
            var sortedModules = method.Invoke(orchestrator, new object[] { modules }) as List<Type>;

            sortedModules.Should().NotBeNull();
            sortedModules.Should().HaveCount(3);
            sortedModules[0].Should().Be(typeof(ModuleA));
            sortedModules[1].Should().Be(typeof(ModuleB));
            sortedModules[2].Should().Be(typeof(ModuleC));
        }

        [Fact]
        public void TopologicalSort_ShouldDetectCircularDependency()
        {
            var fixture = new FluentOrchestratorFixture<TApplication>();
            var orchestrator = fixture.Orchestrator;
            var modules = new List<Type> { typeof(CircularModuleA), typeof(CircularModuleB) };

            var method = orchestrator.GetType().GetMethod("TopologicalSort", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            if (method == null)
            {
                throw new InvalidOperationException("Method TopologicalSort not found.");
            }

            Action act = () => method.Invoke(orchestrator, new object[] { modules });

            act.Should().Throw<TargetInvocationException>()
                .WithInnerException<InvalidOperationException>()
                .WithMessage("Circular dependency detected.");
        }
    }

    public class ModuleA { }

    [ModuleDependency(typeof(ModuleA))]
    public class ModuleB { }

    [ModuleDependency(typeof(ModuleB))]
    public class ModuleC { }

    [ModuleDependency(typeof(CircularModuleB))]
    public class CircularModuleA
    {
        public void Configure() { }
    }

    [ModuleDependency(typeof(CircularModuleA))]
    public class CircularModuleB
    {
        public void Configure() { }
    }
}
