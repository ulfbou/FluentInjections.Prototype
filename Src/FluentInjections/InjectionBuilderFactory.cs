// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections;

internal class DefaultInjectionBuilderFactory<TBuilder> : IInjectionBuilderFactory<TBuilder> where TBuilder : class, IApplicationBuilder<TBuilder>
{
    private string[]? _arguments;

    public DefaultInjectionBuilderFactory(string[]? arguments)
    {
        _arguments = arguments ?? Array.Empty<string>();
    }

    public IInjectionBuilder<TBuilder> CreateBuilder()
    {
        throw new NotImplementedException(); // Do not implement this method.
    }
}
