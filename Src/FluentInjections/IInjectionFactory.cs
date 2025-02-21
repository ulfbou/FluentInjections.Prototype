// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections;

public interface IInjectionFactory
{
    IApplicationBuilder<TBuilder> CreateApplicationBuilder<TBuilder>()
        where TBuilder : class, IApplicationBuilder<TBuilder>, new();

    IApplication<TBuilder> CreateApplication<TBuilder>(TBuilder builder)
        where TBuilder : class, IApplicationBuilder<TBuilder>;

    IBuilderFactory<TBuilder> CreateBuilderFactory<TBuilder>()
        where TBuilder : class, IApplicationBuilder<TBuilder>, new();

    IApplicationBuilderFactory<TApplicationBuilder> CreateApplicationBuilderFactory<TApplicationBuilder>()
            where TApplicationBuilder : class, IApplicationBuilder<TApplicationBuilder>, new();
}
