// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Adapters.AspNetCore;

using Microsoft.Extensions.DependencyInjection;

namespace FluentInjections.Tests.Units.Adapters.AspNetCore
{
    public class WebApplicationBuilderAdapterFixture
    {
        public WebApplicationBuilderAdapter Adapter { get; }
        public ServiceCollection Services { get; }

        public WebApplicationBuilderAdapterFixture()
        {
            Adapter = new WebApplicationBuilderAdapter();
            Services = new ServiceCollection();
        }

        public void AddTestService()
        {
            Services.AddSingleton<ITestService, TestService>();
        }

        internal interface ITestService { }
        internal class TestService : ITestService { }
    }
}