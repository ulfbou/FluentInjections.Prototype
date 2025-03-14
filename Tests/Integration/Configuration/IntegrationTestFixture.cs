// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Castle.Core.Configuration;
using Castle.Core.Logging;

using Microsoft.Extensions.DependencyInjection;

namespace FluentInjections.Tests.Integration.Configuration
{
    public class IntegrationTestFixture<TApplication>
    {
        public IConfiguration Configuration { get; }
        public ILoggerFactory LoggerFactory { get; }
        public IServiceCollection Services { get; }
        public FluentOrchestrator<TApplication> Orchestrator { get; }

        public IntegrationTestFixture()
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json")
                .Build();

            LoggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

            Services = new ServiceCollection();
            Orchestrator = new FluentOrchestrator<TApplication>(null, Configuration, LoggerFactory, Services);
        }
    }
}
