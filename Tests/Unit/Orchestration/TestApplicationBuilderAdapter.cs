// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.DependencyInjection;
using FluentInjections.Core.Discovery.Metadata;
using FluentInjections.Abstractions.Adapters;
using static FluentInjections.Tests.Units.Adapters.AspNetCore.WebApplicationBuilderAdapterFixture;

namespace FluentInjections.Tests.Units.Orchestration
{
    [AdapterMetadata(typeof(TestApplication))]
    public class TestApplicationBuilderAdapter : IApplicationBuilderAdapter<TestApplication>
    {
        public object InnerBuilder => new object();

        public Task<IApplicationAdapter<TestApplication>> BuildAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IApplicationAdapter<TestApplication>>(new TestApplicationAdapter());
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ITestService, TestService>();
        }
        public ValueTask DisposeAsync() => ValueTask.CompletedTask;
    }
}
