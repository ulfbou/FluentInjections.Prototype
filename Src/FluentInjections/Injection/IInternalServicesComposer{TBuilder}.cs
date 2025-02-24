// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Application;
using FluentInjections.Validation;

using Microsoft.Extensions.DependencyInjection;

namespace FluentInjections.Injection
{
    public interface IInternalServicesComposer<TBuilder> where TBuilder : class, IApplicationBuilder<TBuilder>
    {
        IServiceCollection Services { get; }
        IServiceProvider ServiceProvider { get; }

        IInjectionBuilderFactory<TBuilder> CreateFactory(string[]? arguments);
    }

    public class InternalServicesComposer<TBuilder> : IInternalServicesComposer<TBuilder>
        where TBuilder : class, IApplicationBuilder<TBuilder>, new()
    {
        public IServiceCollection Services { get; }
        public IServiceProvider ServiceProvider { get; }

        public InternalServicesComposer(IServiceCollection services, IServiceProvider provider)
        {
            Guard.NotNull(services, nameof(services));
            Guard.NotNull(provider, nameof(provider));
            Services = services;
            ServiceProvider = provider;
        }

        public IInjectionBuilderFactory<TBuilder> CreateFactory(string[]? arguments)
        {
            return new InjectionBuilderFactory<TBuilder>(Services, arguments ?? Array.Empty<string>());
        }
    }
}
