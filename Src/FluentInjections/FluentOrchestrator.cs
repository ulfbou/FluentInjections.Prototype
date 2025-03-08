// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FluentInjections
{
    /// <summary>
    /// Represents a static fluent orchestrator for building applications.
    /// </summary>
    public static class FluentOrchestrator
    {
        /// <summary>
        /// Creates a new fluent orchestrator for the specified application.
        /// </summary>
        /// <typeparam name="TApplication">The type of the application.</typeparam>
        /// <param name="args">The arguments to pass to the application.</param>
        /// <param name="configuration">The configuration to use for the application.</param>
        /// <param name="services">The services to use for the application.</param>
        public static IFluentOrchestrator<TApplication> For<TApplication>(
            IConfiguration? configuration = null,
            IServiceCollection? services = null,
            ILoggerFactory? loggerFactory = null)
            where TApplication : class
        {
            return new FluentOrchestrator<TApplication>(configuration, services, loggerFactory);
        }
    }
}
