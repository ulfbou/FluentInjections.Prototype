// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions.Adapters;
using FluentInjections.Abstractions;
using FluentInjections.Logging;

namespace FluentInjections.Abstractions.Adapters
{
    public interface IApplicationAdapter
    {
        Task RunAsync(CancellationToken? cancellationToken = null);
        Task StopAsync(CancellationToken? cancellationToken = null);
        ILoggerFactoryProvider? LoggerFactoryProvider { get; }
    }
    public interface IApplicationAdapterFactory
    {
        /// <summary>
        /// Creates a concrete builder adapter for the target framework.
        /// </summary>
        /// <returns>An instance of IConcreteBuilderAdapter.</returns>
        IConcreteBuilderAdapter CreateConcreteBuilderAdapter();

        /// <summary>
        /// Creates a concrete application adapter for the target framework.
        /// </summary>
        /// <returns>An instance of IConcreteApplicationAdapter.</returns>
        IConcreteApplicationAdapter CreateConcreteApplicationAdapter(); // Keep this, might be used internally

        /// <summary>
        /// Creates a framework-agnostic IApplication instance, fully configured with adapters and the concrete application.
        /// </summary>
        /// <typeparam name="TBuilder">The type of the framework-agnostic builder abstraction.</typeparam>
        /// <param name="builder">The framework-agnostic builder abstraction instance.</param>
        /// <param name="concreteApplication">The built concrete application instance (framework-specific).</param>
        /// <returns>A framework-agnostic IApplication instance.</returns>
        IApplication<TBuilder> CreateApplication<TBuilder>(TBuilder builder, object concreteApplication)
            where TBuilder : IAppBuilderAbstraction;
    }
}
