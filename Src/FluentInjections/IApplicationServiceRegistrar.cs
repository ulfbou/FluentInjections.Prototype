﻿// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.DependencyInjection;

namespace FluentInjections;

public interface IApplicationServiceRegistrar
{
    void RegisterServices(IServiceCollection services);
}
