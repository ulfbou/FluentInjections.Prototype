// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.DependencyInjection;
using FluentInjections.Validation;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FluentInjections;

internal class MiddlewareConfigurator<TBuilder> : ComponentConfiguratorBase<IMiddlewareComponent>, IMiddlewareConfigurator<TBuilder>
    where TBuilder : IApplicationBuilder<TBuilder>
{

    public MiddlewareConfigurator(IApplication<TBuilder> application, IComponentRegistry<IMiddlewareComponent> innerRegistry, ILoggerFactory loggerFactory)
        : base(innerRegistry, loggerFactory)
    {
        Guard.NotNull(application, nameof(application));
        Application = application;
    }

    public IApplication<TBuilder> Application { get; }


    public IMiddlewareBuilder<TBuilder, TMiddleware> UseMiddleware<TMiddleware>(string? alias, params object[]? arguments)
        where TMiddleware : class, IMiddleware
    {
        var type = typeof(TMiddleware);
        alias ??= type.FullName ?? type.Name;
        var registration = CreateRegistration<TMiddleware>(alias);
        _registrations.Add(registration);
        return new MiddlewareBuilder<TBuilder, TMiddleware, IComponentRegistration<IMiddlewareComponent, TMiddleware>>(Application, registration, _loggerFactory);
    }

    public IMiddlewareConfigurator<TBuilder> Use<TMiddleware>(string? alias, params object[]? arguments)
        where TMiddleware : class
    {
        var type = typeof(TMiddleware);
        alias ??= type.FullName ?? type.Name;
        var typeName = typeof(TMiddleware);
        var registration = CreateRegistration<TMiddleware>(alias);
        _registrations.Add(registration);
        return this;
    }

    protected override IComponentBuilder<IMiddlewareComponent, TMiddleware> CreateBuilder<TMiddleware>(IComponentRegistration<IMiddlewareComponent, TMiddleware> registration)
    {
        return new MiddlewareBuilder<TBuilder, TMiddleware, IComponentRegistration<IMiddlewareComponent, TMiddleware>>(Application, registration, _loggerFactory);
    }

    protected override IComponentRegistration<IMiddlewareComponent, object> CreateRegistration(Type componentType, string alias)
    {
        return new MiddlewareRegistration<object>
        {
            ContractType = componentType,
            Alias = alias
        };
    }

    protected override IComponentRegistration<IMiddlewareComponent, TContract> CreateRegistration<TContract>(string alias)
    {
        return new MiddlewareRegistration<TContract>
        {
            ContractType = typeof(TContract),
            Alias = alias
        };
    }
}
