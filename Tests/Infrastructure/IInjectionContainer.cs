// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.DependencyInjection;

namespace FluentInjections.Tests.Infrastructure
{
    public class InjectionContainer : IInjectionContainer
    {
        public InjectionContainer(WebApplicationBuilder builder)
        {
            Builder = builder;
            Builder.
        }

        public WebApplicationBuilder Builder { get; }
        public IServiceCollection Services => Builder.Services;
        public IServiceProvider Provider => Services.BuildServiceProvider();
        public
    }
}