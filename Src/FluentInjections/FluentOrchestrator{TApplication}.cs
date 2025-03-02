// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.


using FluentInjections.Abstractions;
using FluentInjections.Discovery;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using System.Reflection;

namespace FluentInjections
{

    /// <summary>
    /// Represents a fluent orchestrator for building applications.
    /// </summary>
    /// <typeparam name="TApplication">The type of the application.</typeparam>
    public class FluentOrchestrator<TApplication> where TApplication : class
    {
        private string[] _arguments;
        private IServiceCollection _services;
        private ILoggerFactory _loggerFactory;
        private IConfiguration _configuration;
        private readonly Dictionary<string, Abstractions.Metadata.IAdapterTypeMetadata> _adapterRegistry = new(); // Does not need to consider thread safety!

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentOrchestrator{TApplication}"/> class.
        /// </summary>
        /// <param name="arguments">The arguments to pass to the application.</param>
        /// <param name="configuration">The configuration to use for the application.</param>
        /// <param name="services">The services to use for the application.</param>
        /// <param name="loggerFactory">The logger factory to use for the application.</param>
        public FluentOrchestrator(string[]? arguments = null, IConfiguration? configuration = null, IServiceCollection? services = null, ILoggerFactory? loggerFactory = null)
        {
            _arguments = arguments ?? Array.Empty<string>();
            _configuration = configuration ?? new ConfigurationBuilder().Build();
            _services = services ?? new ServiceCollection();
            _loggerFactory = loggerFactory ?? LoggerFactory.Create(builder => builder.AddConsole());
        }

        /// <summary>
        /// Adds a service to the application.
        /// </summary>
        /// <typeparam name="TAdapter">The type of the adapter to use.</typeparam>
        /// <param name="frameworkIdentifier">The identifier of the framework to use the adapter for.</param>
        /// <returns>A <see cref="FluentOrchestrator{TApplication}"/> instance.</returns>
        public FluentOrchestrator<TApplication> UseAdapter<TAdapter>(string frameworkIdentifier)
            where TAdapter : class
        {
            var adapterType = typeof(TAdapter);
            var metadataAttribute = adapterType.GetCustomAttribute<AdapterMetadataAttribute>();

            if (metadataAttribute == null)
            {
                throw new InvalidOperationException($"Adapter type {adapterType.FullName} does not have an AdapterMetadataAttribute.");
            }

            _adapterRegistry[frameworkIdentifier] = metadataAttribute;

            return this;
        }

        /// <summary>
        /// Adds a service to the application.
        /// </summary>
        /// <param name="service">The service to add.</param>
        /// <returns>A <see cref="FluentOrchestrator{TApplication}"/> instance.</returns>
        public Task<IApplicationAbstraction> BuildAsync(CancellationToken cancellationToken = default)
        {
            // Implementation to be added later
            // Use IBuilderFactory and IApplicationBuilderAdapterFactory
            // Transfer services, etc.
            return null!; // Placeholder
        }

        // Methods for adapter usage and discovery directives will be added later
    }
}
