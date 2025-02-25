// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Application;

using Microsoft.Extensions.DependencyInjection;

namespace FluentInjections.Internal
{
    public interface IInjectionBuilderFactory<TBuilder>
        where TBuilder : IApplicationBuilder<TBuilder>
    {
        IInjectionBuilder<TBuilder> CreateBuilder();
    }
}
