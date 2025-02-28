// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;

namespace FluentInjections.Tests.Utils
{
    public interface IWebApplicationBuilder : IHostApplicationBuilder
    {
        IHost Build();
    }
}
