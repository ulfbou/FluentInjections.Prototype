// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Application;
using FluentInjections.Collections;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using System.Reflection;

namespace FluentInjections.Injection
{
    /// <summary>
    /// Represents the builder for configuring the injection.
    /// </summary>
    /// <typeparam name="TBuilder">The type of the builder to be configured.</typeparam>
    /// <typeparam name="TApplication">The type of the application to be built.</typeparam>
    /// <remarks>
    /// This interface is used to configure the infrastructure components within the application, as well as building the application.
    /// </remarks>
    public interface IInjectionBuilder<TBuilder> : IApplicationBuilder<TBuilder>, IDisposable
        where TBuilder : IApplicationBuilder<TBuilder>
    {
        /// <summary>
        /// Registers assemblies that contains <see cref="IConfigurableModule{ILifecycleConfigurator}"/> to be discovered. 
        /// </summary>
        /// <param name="assemblies">The assemblies to be discovered.</param>
        /// <returns>The <see cref="IInjectionBuilder{TBuilder}"/> instance.</returns>
        IInjectionBuilder<TBuilder> WithLifecycles(Action<AssemblyCollection> configure);

        /// <summary>
        /// Registers assemblies that contains <see cref="IConfigurableModule{IMiddlewareConfigurator}"/> to be discovered. 
        /// </summary>
        /// <param name="assemblies">The assemblies to be discovered.</param>
        /// <returns>The <see cref="IInjectionBuilder{TBuilder}"/> instance.</returns>
        IInjectionBuilder<TBuilder> WithMiddlewares(Action<AssemblyCollection> configure);

        /// <summary>
        /// Registers assemblies that contains <see cref="IConfigurableModule{IServiceConfigurator}"/> to be discovered.
        /// </summary>
        /// <param name="assemblies">The assemblies to be discovered.</param>
        /// <returns>The <see cref="IInjectionBuilder{TBuilder}"/> instance.</returns>
        IInjectionBuilder<TBuilder> WithServices(Action<AssemblyCollection> configure);

        /// <summary>
        /// Registers assemblies that contains <see cref="IConfigurableModule{IApplicationConfigurator}"/> to be discovered.
        /// </summary>
        /// <param name="assemblies">The assemblies to be discovered.</param>
        /// <returns>The <see cref="IInjectionBuilder{TBuilder}"/> instance.</returns>
        IInjectionBuilder<TBuilder> WithServiceCollection(IServiceCollection services);

        /// <summary>
        /// Registers assemblies that contains <see cref="IConfigurableModule{IApplicationConfigurator}"/> to be discovered.
        /// </summary>
        /// <param name="assemblies">The assemblies to be discovered.</param>
        /// <returns>The <see cref="IInjectionBuilder{TBuilder}"/> instance.</returns>
        IInjectionBuilder<TBuilder> WithServiceProvider(IServiceProvider serviceProvider);

        /// <summary>
        /// Builds the application asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="IApplication{TBuilder}"/> instance.</returns>
        Task<IApplication<TBuilder>> BuildAsync(CancellationToken cancellationToken = default);
    }
}
