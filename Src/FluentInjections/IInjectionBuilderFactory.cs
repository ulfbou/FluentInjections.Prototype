// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections
{
    public interface IInjectionBuilderFactory<TBuilder> where TBuilder : class, IApplicationBuilder<TBuilder>
    {
        IInjectionBuilder<TBuilder> CreateBuilder();
    }
}