// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentAssertions;

namespace FluentInjections.Tests.Integration.Configuration
{
    public class ConfigurationDiscoveryIntegrationTests
    {
        [Fact]
        public async System.Threading.Tasks.Task BuildAsync_ShouldDiscoverModulesBasedOnConfiguration()
        {
            var fixture = new IntegrationTestFixture<TestApplication>();
            var application = await fixture.Orchestrator.BuildAsync();

            var serviceProvider = fixture.Services.BuildServiceProvider();

            // Verify that TestModuleA is registered as ITestModule.
            serviceProvider.GetService<ITestModule>().Should().BeOfType<TestModuleA>();

            // Verify that TestModuleB is not registered.
            serviceProvider.GetService<TestModuleB>().Should().BeNull();
        }
    }
    public class TestApplication { }
}