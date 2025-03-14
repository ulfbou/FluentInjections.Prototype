// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions;
using FluentInjections.Registration;
using FluentInjections.Validation;

using Microsoft.Extensions.Logging;

namespace FluentInjections.Internal.Registration
{
    internal class ServiceConfigurator : ComponentConfigurator<IServiceComponent>, IServiceConfigurator, IConfigurator<IServiceComponent>
    {
        public ServiceConfigurator(ILoggerFactory loggerFactory) : base(loggerFactory) { }

        public async ValueTask<IComponentBuilder<IServiceComponent, TContract>> RegisterAsync<TContract>(string? alias = null, CancellationToken? cancellationToken = null)
        {
            return await RegisterInternalAsync<TContract, ComponentBuilder<IServiceComponent, TContract>>(alias, cancellationToken);
        }

        public async ValueTask<IComponentBuilder<IServiceComponent, object>> RegisterAsync(string? alias = null, CancellationToken? cancellationToken = null)
        {
            return await RegisterInternalAsync<object, ComponentBuilder<IServiceComponent, object>>(alias, cancellationToken);
        }

        public Task RegisterAsync(CancellationToken cancellationToken) => throw new NotImplementedException();

        protected override ValueTask<TBuilder> CreateBuilderAsync<TContract, TBuilder>(ServiceRegistration<TContract> registration, CancellationToken? cancellationToken)
        {
            return ValueTask.FromResult<TBuilder>((TBuilder)(object)new ServiceComponentBuilder<TContract>(registration, _loggerFactory));
        }
    }
}
