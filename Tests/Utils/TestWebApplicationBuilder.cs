// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Diagnostics.Metrics;

using Moq;
using Microsoft.AspNetCore.Builder;

namespace FluentInjections.Tests.Utils
{
    public class TestWebApplicationBuilder : IWebApplicationBuilder
    {
        private readonly Mock<IWebApplicationBuilder> _mockWebApplicationBuilder;
        private readonly Mock<IHost> _mockHost;

        public IWebApplicationBuilder WebApplicationBuilder => _mockWebApplicationBuilder.Object;
        public IHostApplicationBuilder HostApplicationBuilder => _mockWebApplicationBuilder.Object;

        public IConfigurationManager Configuration => WebApplicationBuilder.Configuration;
        public IHostEnvironment Environment => WebApplicationBuilder.Environment;
        public ILoggingBuilder Logging => WebApplicationBuilder.Logging;
        public IMetricsBuilder Metrics => WebApplicationBuilder.Metrics;
        public IDictionary<object, object> Properties => WebApplicationBuilder.Properties;
        public IServiceCollection Services => WebApplicationBuilder.Services;

        public TestWebApplicationBuilder()
        {
            _mockWebApplicationBuilder = new Mock<IWebApplicationBuilder>();
            _mockHost = new Mock<IHost>();
            _mockWebApplicationBuilder.Setup(builder => builder.Services).Returns(new ServiceCollection());
            _mockWebApplicationBuilder.Setup(builder => builder.Build()).Returns(_mockHost.Object);
            _mockWebApplicationBuilder.Setup(builder => builder.Configuration).Returns(new ConfigurationManager());
            _mockWebApplicationBuilder.Setup(builder => builder.Properties).Returns(new Dictionary<object, object>());
            _mockWebApplicationBuilder.Setup(builder => builder.ConfigureContainer(It.IsAny<IServiceProviderFactory<IServiceCollection>>(), It.IsAny<Action<IServiceCollection>>())).Callback<IServiceProviderFactory<IServiceCollection>, Action<IServiceCollection>>((factory, configure) => factory.CreateBuilder(new ServiceCollection()));
        }

        public IHost Build() => WebApplicationBuilder.Build();

        public void ConfigureContainer<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> factory, Action<TContainerBuilder>? configure = null) where TContainerBuilder : notnull
        {
            WebApplicationBuilder.ConfigureContainer(factory, configure);
        }
    }
}
