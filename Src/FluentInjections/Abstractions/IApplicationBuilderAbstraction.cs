// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.DependencyInjection;

namespace FluentInjections.Abstractions
{
    /// <summary>
    /// Represents a framework-agnostic application builder abstraction.
    /// Implementations are responsible for providing access to the appropriate IApplicationAdapterFactory.
    /// </summary>
    public interface IApplicationBuilderAbstraction
    {
        /// <summary>
        /// Gets the framework identifier for this builder abstraction.
        /// </summary>
        string FrameworkIdentifier { get; }

        /// <summary>
        /// Gets the Type of the IApplicationAdapterFactory to be used with this builder abstraction.
        /// </summary>
        Type AdapterFactoryType { get; }

        /// <summary>
        /// Gets the type of the concrete application built by this abstraction.
        /// </summary>
        Type ConcreteApplicationType { get; }

        /// <summary>
        /// Builds the application asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation. The task result is the built application.</returns>
        Task<IApplicationAbstraction> BuildAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Configures the service components for the application.
        /// </summary>
        /// <typeparam name="TRegistry">The type of the service component registry.</typeparam>
        /// <param name="configureRegistry">The action to configure the service component registry.</param>
        /// <returns>The application builder abstraction instance.</returns>
        IApplicationBuilderAbstraction ConfigureServices<TRegistry>(Action<TRegistry> configureRegistry)
            where TRegistry : DependencyInjection.IComponentRegistry<IServiceComponent>;

        /// <summary>
        /// Configures the middleware components for the application.
        /// </summary>
        /// <typeparam name="TRegistry">The type of the middleware component registry.</typeparam>
        /// <param name="configureMiddleware">The action to configure the middleware component registry.</param>
        /// <returns>The application builder abstraction instance.</returns>
        IApplicationBuilderAbstraction ConfigureMiddleware<TRegistry>(Action<TRegistry> configureMiddleware)
            where TRegistry : DependencyInjection.IComponentRegistry<IMiddlewareComponent>;
    }
}
