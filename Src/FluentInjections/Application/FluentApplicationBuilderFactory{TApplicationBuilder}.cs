// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace FluentInjections.Application
{
    public class FluentApplicationBuilderFactory<TApplicationBuilder> : IApplicationBuilderFactory<TApplicationBuilder>
        where TApplicationBuilder : class, IApplicationBuilder<TApplicationBuilder>, new()
    {
        public TApplicationBuilder CreateApplicationBuilder(string[]? arguments, IServiceCollection? externalServices)
        {
            var builder = new TApplicationBuilder();

            if (externalServices != null)
            {
                foreach (var serviceDescriptor in externalServices)
                {
                    builder.Services.Add(serviceDescriptor);
                }
            }

            return builder;
        }
    }
}
