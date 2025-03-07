// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions;
using FluentInjections.Internal.Registration;

using Microsoft.Extensions.Logging;

namespace FluentInjections.Registration
{
    internal class ServiceComponentBuilder<TContract>
            : ComponentBuilder<IServiceComponent, TContract>, IServiceComponentBuilder<TContract>, IComponentBuilder<IServiceComponent, TContract>
    {
        public ServiceComponentBuilder(IServiceRegistration<TContract> registration, ILoggerFactory loggerFactory)
            : base(registration, loggerFactory)
        { }
    }
}
