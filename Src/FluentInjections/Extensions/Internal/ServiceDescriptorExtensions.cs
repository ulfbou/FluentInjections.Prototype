// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Internal.Registration;

namespace FluentInjections.Extensions.Internal
{
    internal static class ServiceDescriptorExtensions
    {
        //internal static IComponentRegistration<IServiceComponent, TContract> ToRegistration<TContract>(this ServiceDescriptor descriptor, string alias)
        //{
        //    // Ensure that service type and contract type are the same
        //    if (descriptor.ServiceType != typeof(TContract))
        //    {
        //        throw new InvalidOperationException("Service type and contract type must be the same");
        //    }

        //    /*
        //             public ComponentLifetime Lifetime { get; set; }
        //            public Type ContractType { get; set; } = typeof(TContract);
        //            public required string Alias { get; set; }
        //            public TContract? Instance { get; set; }
        //            public Func<IComponentResolver<TComponent>, CancellationToken, ValueTask<TContract>>? Factory { get; set; }
        //            public Func<TContract, CancellationToken, ValueTask>? Configure { get; set; }
        //            public IDictionary<string, object?> Metadata { get; } = new Dictionary<string, object?>();
        //            public object? Parameters { get; set; }
        //            public Type? ResolutionType { get; set; }
        //            public Func<IServiceProvider, CancellationToken, ValueTask<bool>>? Condition { get; set; }

        //     */
        //    Func<IComponentResolver<IServiceComponent>, CancellationToken, ValueTask<TContract>>? factory = default;

        //    if (descriptor.ImplementationFactory is Func<IServiceProvider, TContract> typedFactory)
        //    {
        //        factory = (resolver, cancellationToken) => ValueTask.FromResult<TContract>((TContract)typedFactory(default!));
        //    }

        //    //    IComponentResolver<TComponent> resolver,
        //    //    string? alias = null,
        //    //    ComponentLifetime lifetime = ComponentLifetime.Transient,
        //    //    Func<IComponentResolver<TComponent>, CancellationToken, ValueTask<TContract>>? factory = null,
        //    //    Func< TContract, CancellationToken, ValueTask >? configure = null,
        //    //    TContract? instance = default,
        //    //    IDictionary< string, object?>? metadata = null,
        //    //    object? parameters = null,
        //    //    Type? resolutionType = null,
        //    //    Func<IServiceProvider, CancellationToken, ValueTask<bool>>? condition = null)
        //    return ComponentRegistration.Create<IServiceComponent, TContract>(alias, descriptor.Lifetime.ToComponentLifetime(), factory, descriptor.ImplementationInstance);
        //    //{
        //    //    Alias = alias,
        //    //    Lifetime = descriptor.Lifetime.ToComponentLifetime(),
        //    //    ContractType = descriptor.ServiceType,
        //    //    Instance = descriptor.ImplementationInstance is TContract instance ? instance : default,
        //    //    Factory = factory,
        //    //};
        //}
    }
}
