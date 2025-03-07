// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.


using FluentInjections.Abstractions;
using FluentInjections.DependencyInjection;
using FluentInjections.Internal.Descriptors;
using FluentInjections.Internal.Registration;

using Microsoft.Extensions.DependencyInjection;

namespace FluentInjections.Registration
{
    internal static class ServiceRegistration
    {
        //public static IServiceRegistration<TContract> Create<TContract>(
        //    IComponentResolver<IServiceComponent> resolver,
        //    string? alias = null,
        //    ComponentLifetime lifetime = ComponentLifetime.Transient)
        //{
        //    return new ServiceRegistration<TContract>();
        //}
    }

    internal class ServiceRegistration<TContract> : ComponentRegistration<IServiceComponent, TContract>, IServiceRegistration<TContract> { }
}
