// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Builder;
using FluentInjections.Adapters.AspNetCore;

namespace FluentInjections.Tests.Units.Core.Discovery
{
    public class ApplicationTypeRegistryFixture
    {
        public ApplicationTypeRegistry Registry { get; }

        public ApplicationTypeRegistryFixture()
        {
            Registry = new ApplicationTypeRegistry();
        }

        public WebApplicationBuilderAdapter CreateWebApplicationBuilder()
        {
            return new WebApplicationBuilderAdapter();
        }
    }
}
