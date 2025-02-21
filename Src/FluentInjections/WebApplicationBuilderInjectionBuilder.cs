// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.


using FluentInjections.Collections;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting; // For IWebHostEnvironment
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace FluentInjections;

internal class WebApplicationBuilderInjectionBuilder : InjectionBuilderBase<WebApplicationBuilder>
{
    private readonly ILogger<WebApplicationBuilderInjectionBuilder> _logger;
    private readonly IWebHostEnvironment _webHostEnvironment; // For environment-specific config
    private readonly IConfiguration _configuration;

    public WebApplicationBuilderInjectionBuilder(
        IServiceProvider internalServiceProvider,
        WebApplicationBuilder builder,
        IServiceProviderFactory serviceProviderFactory,
        IWebHostEnvironment webHostEnvironment, // Inject IWebHostEnvironment
        IConfiguration configuration) : base(internalServiceProvider, builder, serviceProviderFactory)
    {
        _logger = internalServiceProvider.GetRequiredService<ILogger<WebApplicationBuilderInjectionBuilder>>();
        _webHostEnvironment = webHostEnvironment;
        _configuration = configuration;
    }

    public override async Task<IApplication<WebApplicationBuilder>> BuildAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _logger.LogInformation("Building WebApplication...");

        await RegisterInternalModulesAsync(cancellationToken);
        await RegisterExternalModulesAsync(cancellationToken);

        _logger.LogDebug("Applying service configurations...");

        // Apply service configurations

        _serviceProvider = _serviceProviderFactory != null
            ? _serviceProviderFactory.CreateServiceProvider(_services)
            : _services.BuildServiceProvider();
        var logger = _serviceProvider.GetRequiredService<ILogger<AssemblyCollection>>();
        var assemblies = new AssemblyCollection(logger);
        _configureServicesAction(assemblies);

        // Build the service provider.
        _serviceProvider = _serviceProviderFactory != null
            ? _serviceProviderFactory.CreateServiceProvider(_services)
            : _services.BuildServiceProvider();

        // Configure the inner builder with the service provider.
        _builder.Services.Add(_services); // Add internal services
        _builder.Host.ConfigureServices(_ => { }); // Dummy call to force service provider creation.

        // Configure host and web host if needed.
        ConfigureHost(_builder.InnerBuilder.Host);
        ConfigureWebHost(_builder.InnerBuilder.WebHost);

        _logger.LogDebug("Applying middleware configurations...");

        // Apply middleware configurations
        _configureMiddlewaresAction(assemblies);

        _logger.LogDebug("Applying lifecycle configurations...");

        // Apply lifecycle configurations
        _configureLifecyclesAction(assemblies);

        _logger.LogDebug("Executing application builder...");

        // Execute the application builder.
        await _applicationBuilderExecutor.ExecuteAsync(_builder, _serviceProvider, cancellationToken);


        _logger.LogInformation("WebApplication built successfully.");

        return CreateApplication(_builder);
    }

    protected virtual void ConfigureHost(IHostBuilder host)
    {
        // Example: Configure host using _configuration and _webHostEnvironment
        host.ConfigureAppConfiguration((hostingContext, config) =>
        {
            // Add your configuration sources here.
            // config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        });

        // Example: Configure logging
        host.ConfigureLogging((hostingContext, logging) =>
        {
            logging.AddConfiguration(_configuration.GetSection("Logging"));
            logging.AddConsole();
        });
    }

    protected virtual void ConfigureWebHost(IWebHostBuilder webHost)
    {
        // Example: Configure web host
        webHost.ConfigureKestrel(serverOptions =>
        {
            // Configure Kestrel options if needed.
        });
    }


    protected override IApplication<WebApplicationBuilder> CreateApplication(WebApplicationBuilder builder)
    {
        return new WebApplication(builder);
    }

    protected override async Task RegisterExternalModulesAsync(CancellationToken cancellationToken)
    {
        _logger.LogDebug("Registering external modules...");

        // 1. Discover Assemblies (using your IAssemblyManager or similar)
        var assemblies = new List<Assembly>(); // Replace with your assembly discovery logic.

        // 2. Register Modules
        var moduleRegistry = _innerProvider.GetRequiredService<IModuleRegistry>(); // Get the module registry
        foreach (var assembly in assemblies)
        {
            var modules = assembly.GetTypes()
                .Where(t => typeof(IModule).IsAssignableFrom(t) && !t.IsAbstract)
                .Select(Activator.CreateInstance)
                .Cast<IModule>();

            foreach (var module in modules)
            {
                // Determine the configurator type for the module (if needed)
                // Example:
                // if (module is IConfigurableModule<YourConfigurator> configurableModule)
                // {
                //     await moduleRegistry.RegisterAsync(configurableModule, cancellationToken);
                // }

                // Or, register directly if the module doesn't need configuration.
                if (module is IInitializable initializableModule)
                {
                    await moduleRegistry.RegisterAsync(typeof(WebApplicationBuilderInjectionBuilder), module, cancellationToken); // Or the appropriate configurator type
                }
            }
        }

        _logger.LogDebug("External modules registered.");
    }
}