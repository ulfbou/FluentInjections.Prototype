// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.Logging;

using System.Diagnostics.Contracts;

namespace FluentInjections;

internal class ServiceBuilder<TContract, TRegistration>
    : ComponentBuilderBase<IServiceComponent, TContract, TRegistration>
    , IServiceBuilder, IComponentBuilder<IServiceComponent, TContract>
        where TRegistration : IComponentRegistration<IServiceComponent, TContract>
{
    public ServiceBuilder(TRegistration registration, ILoggerFactory loggerFactory)
        : base(registration, loggerFactory)
    { }
}
