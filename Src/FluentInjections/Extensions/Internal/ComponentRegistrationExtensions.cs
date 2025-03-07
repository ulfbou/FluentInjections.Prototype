// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.DependencyInjection;
using FluentInjections.Abstractions;
using FluentInjections.Validation;

namespace FluentInjections.Extensions.Internal
{
    public static class ComponentRegistrationExtensions
    {
        //public static Func<IServiceProvider, CancellationToken, ValueTask<TContract?>> AdaptFactory<TComponent, TContract>(
        //    this IComponentRegistration<TComponent, TContract> descriptor,
        //    CancellationToken cancellationToken = default)
        //    where TComponent : IComponent
        //{
        //    Guard.NotNull(descriptor, nameof(descriptor));

        //    if (descriptor.Factory is null)
        //    {
        //        throw new InvalidOperationException("The factory function is not set.");
        //    }

        //    return async (_, cancellationToken) =>
        //   {
        //       var resolverServiceProvider = new ResolverServiceProvider<TComponent>(resolver, serviceProviderFactory, advancedServiceProvider);
        //       return await descriptor.Factory(resolver, cancellationToken);
        //   };
        //}

        public static Func<IServiceProvider, CancellationToken, ValueTask<TContract?>> AdaptFactory<TComponent, TContract>(
                this IComponentDescriptor<TComponent, TContract> descriptor,
                IComponentResolver<TComponent> resolver,
                CancellationToken cancellationToken = default)
            where TComponent : IComponent
        {
            if (descriptor.Factory is null)
            {
                return async (serviceProvider, cancellationToken) =>
                {
                    var instance = descriptor.Instance;

                    if (instance is null)
                    {
                        return default;
                    }
                    if (descriptor.Configure is not null)
                    {
                        await descriptor.Configure(instance, cancellationToken);
                    }
                    return instance;
                };
            }

            return async (serviceProvider, cancellationToken) =>
            {
                var instance = await descriptor.Factory(resolver, cancellationToken);
                if (descriptor.Configure is not null)
                {
                    await descriptor.Configure(instance, cancellationToken);
                }
                return instance;
            };
        }
    }
}
