// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using FluentInjections.Orchestration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using FluentInjections.Core.Discovery;
using FluentInjections.Core.Configuration;
using System;
using System.Threading;
using FluentInjections.Abstractions.Adapters;
using FluentInjections.Attributes;
using System.Linq;
using FluentInjections.Core.Abstractions;

namespace FluentInjections.Tests.Units.Orchestration
{
    public class FluentOrchestratorFixture<TApplication>
    {
        public Mock<IConfiguration> MockConfiguration { get; }
        public Mock<ILogger> MockLogger { get; }
        public Mock<ILogger<FluentOrchestrator<TestApplication>>> MockLoggerOrchestrator { get; }
        public Mock<ILoggerFactory> MockLoggerFactory { get; }
        public Mock<TypeDiscoveryPipeline> MockTypeDiscoveryPipeline { get; }
        public Mock<IApplicationBuilderAdapter<TApplication>> MockApplicationBuilderAdapter { get; }
        public Mock<IApplicationAdapter<TApplication>> MockApplicationAdapter { get; }
        public Mock<IServiceProvider> MockServiceProvider { get; }
        public TypeDiscoveryOptions DiscoveryOptions { get; }
        public ServiceCollection Services { get; }
        public FluentOrchestrator<TApplication> Orchestrator { get; private set; }

        public FluentOrchestratorFixture()
        {
            MockConfiguration = new Mock<IConfiguration>();

            MockLogger = new Mock<ILogger>();
            MockLoggerOrchestrator = new Mock<ILogger<FluentOrchestrator<TestApplication>>>();
            MockLoggerFactory = new Mock<ILoggerFactory>();

            // Setup the non-generic CreateLogger to return the mock loggers.
            MockLoggerFactory
                .Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(new Mock<ILogger>().Object);

            MockLoggerFactory
                .Setup(x => x.CreateLogger(typeof(AttributeDiscoveryStrategy).FullName ?? typeof(AttributeDiscoveryStrategy).Name))
                .Returns(MockLogger.Object);

            MockLoggerFactory
                .Setup(x => x.CreateLogger(typeof(FluentOrchestrator<TestApplication>).FullName ?? typeof(FluentOrchestrator<TestApplication>).Name))
                .Returns(MockLoggerOrchestrator.Object);

            MockTypeDiscoveryPipeline = new Mock<TypeDiscoveryPipeline>();
            MockApplicationBuilderAdapter = new Mock<IApplicationBuilderAdapter<TApplication>>();
            MockApplicationAdapter = new Mock<IApplicationAdapter<TApplication>>();
            MockServiceProvider = new Mock<IServiceProvider>();
            DiscoveryOptions = TypeDiscoveryOptions.DefaultOptions;
            Services = new ServiceCollection();
            Orchestrator = CreateOrchestrator();
        }

        public FluentOrchestrator<TApplication> CreateOrchestrator(string[]? arguments = null)
        {
            return new FluentOrchestrator<TApplication>(arguments, MockConfiguration.Object, MockLoggerFactory.Object, Services);
        }

        public void SetupConfiguration(string section, Mock<IConfigurationSection> mockConfigurationSection)
        {
            MockConfiguration.Setup(c => c.GetSection(section))
                             .Returns(mockConfigurationSection.Object);
        }

        public void SetupAdapter(string frameworkIdentifier = "Test")
        {
            Orchestrator.UseAdapterInstance(frameworkIdentifier, MockApplicationBuilderAdapter.Object);
        }

        public void SetupAdapter<TAdapter>(string frameworkIdentifier = "Test") where TAdapter : IApplicationAdapter<TApplication>
        {
            Orchestrator.UseAdapter<TAdapter>(frameworkIdentifier);
        }

        internal void RegisterMockAdapterIntoServices(IApplicationBuilderAdapter<TApplication> mockAdapter)
        {
            Services.AddSingleton(mockAdapter);
        }

        public void SetupDiscoveryPipeline(IEnumerable<Type> discoveredTypes)
        {
            MockTypeDiscoveryPipeline.Setup(p => p.DiscoverTypes(It.IsAny<TypeDiscoveryContext>(), It.IsAny<CancellationToken>()))
                                     .Returns(discoveredTypes.ToAsyncEnumerable());
        }

        public void SetupApplicationBuilderAdapter(TApplication application)
        {
            MockApplicationAdapter.Setup(a => a.Application).Returns(application);
            MockApplicationBuilderAdapter.Setup(b => b.BuildAsync(It.IsAny<CancellationToken>()))
                //.Callback((CancellationToken token) => MockApplicationBuilderAdapter.Object.ConfigureServices(new ServiceCollection()))
                .ReturnsAsync(MockApplicationAdapter.Object);
        }

        public void SetupServiceProvider(IServiceProvider serviceProvider)
        {
            MockServiceProvider.Setup(sp => sp.GetService(It.IsAny<Type>())).Returns(serviceProvider.GetService(It.IsAny<Type>()));
            MockServiceProvider.Setup(sp => sp.GetServices(It.IsAny<Type>())).Returns(serviceProvider.GetServices(It.IsAny<Type>()));
            MockServiceProvider.Setup(sp => sp.GetRequiredService(It.IsAny<Type>())).Returns(serviceProvider.GetRequiredService(It.IsAny<Type>()));
        }

        public void SetupDiscoveryOptions(TypeDiscoveryOptions options)
        {
            Orchestrator.WithDiscoveryOptions(options);
        }

        public void SetupDiscoveredAdapters(IEnumerable<Type> adapterTypes)
        {
            SetupDiscoveryPipeline(adapterTypes);
        }

        public void SetupDiscoveredModules(IEnumerable<Type> moduleTypes)
        {
            SetupDiscoveryPipeline(moduleTypes);
        }

        public void SetupModuleDependencies(Type moduleType, IEnumerable<Type> dependencyTypes)
        {
            var attributes = dependencyTypes.Select(dt => new ModuleDependencyAttribute(dt)).ToArray();
            Mock.Get(moduleType).Setup(m => m.GetCustomAttributes(typeof(ModuleDependencyAttribute), false)).Returns(attributes);
        }

        public void SetupFluentInjectionsConfiguration(string sectionName = "FluentInjections", IConfigurationSection section = null!)
        {
            MockConfiguration.Setup(c => c.GetSection(sectionName)).Returns(section ?? new Mock<IConfigurationSection>().Object);
        }

        public TestApplicationBuilderAdapter CreateTestAdapter()
        {
            return new TestApplicationBuilderAdapter();
        }

        public TestApplicationAdapter CreateTestApplicationAdapter()
        {
            return new TestApplicationAdapter();
        }
    }
}
