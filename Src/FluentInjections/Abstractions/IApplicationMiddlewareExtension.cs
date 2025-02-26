// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Adapters;

using Microsoft.AspNetCore.Builder;

namespace FluentInjections.Abstractions
{
    public interface IApplicationMiddlewareExtension<TConcreteApplicationAdapter>
        where TConcreteApplicationAdapter : IApplicationAdapter
    {
        TConcreteApplicationAdapter UseMiddleware(Func<ApplicationDelegate, ApplicationDelegate> middleware);
    }
}
