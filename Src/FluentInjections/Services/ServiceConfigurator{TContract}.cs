// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Configurators;

using Microsoft.Extensions.DependencyInjection;

namespace FluentInjections.Services
{
    public class ServiceConfigurator : ConfiguratorBase<IServiceComponent>, IServiceConfigurator
    {
        public ServiceConfigurator(IServiceCollection services) : base(services) { }

        public IServiceCollection Services => _services;
    }
}
