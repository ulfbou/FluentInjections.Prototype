//using FluentInjections.Application;
//using FluentInjections.Collections;

//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.DependencyInjection.Extensions;
//using Microsoft.Extensions.Logging;
//// Copyright (c) FluentInjections Project. All rights reserved.
//// Licensed under the MIT License. See LICENSE in the project root for license information.


//namespace FluentInjections
//{
//public class InjectionBuilderBase<T>
//{
//    private IServiceProvider internalServiceProvider;
//    private WebApplicationBuilder builder;
//    private IServiceProviderFactory serviceProviderFactory;

//    public InjectionBuilderBase(IServiceProvider internalServiceProvider, WebApplicationBuilder builder, IServiceProviderFactory serviceProviderFactory)
//    {
//        this.internalServiceProvider = internalServiceProvider;
//        this.builder = builder;
//        this.serviceProviderFactory = serviceProviderFactory;
//    }

//    public override async Task<IApplication<WebApplicationBuilder>> BuildAsync(CancellationToken cancellationToken)
//    {
//        cancellationToken.ThrowIfCancellationRequested();

//        _logger.LogInformation("Building WebApplication...");

//        await RegisterInternalModulesAsync(cancellationToken);
//        await RegisterExternalModulesAsync(cancellationToken);

//        _logger.LogDebug("Applying service configurations...");

//        // Apply service configurations

//        _serviceProvider = _serviceProviderFactory != null
//            ? _serviceProviderFactory.CreateServiceProvider(_services)
//            : _services.BuildServiceProvider();
//        var logger = _serviceProvider.GetRequiredService<ILogger<AssemblyCollection>>();
//        var assemblies = new AssemblyCollection(logger);
//        _configureServicesAction(assemblies);

//        // Build the service provider.
//        _serviceProvider = _serviceProviderFactory != null
//            ? _serviceProviderFactory.CreateServiceProvider(_services)
//            : _services.BuildServiceProvider();

//        // Configure the inner builder with the service provider.
//        _builder.Services.Add(_services); // Add internal services

//        // Configure host and web host if needed.
//        ConfigureHost(_builder.GetInnerBuilder<Microsoft.Extensions.Hosting.IHostBuilder>());
//        ConfigureWebHost(_builder.GetInnerBuilder<Microsoft.AspNetCore.Hosting.IWebHostBuilder>());

//        _logger.LogDebug("Applying middleware configurations...");

//        // Apply middleware configurations
//        _configureMiddlewaresAction(assemblies);

//        _logger.LogDebug("Applying lifecycle configurations...");

//        // Apply lifecycle configurations
//        _configureLifecyclesAction(assemblies);

//        _logger.LogDebug("Executing application builder...");

//        // Execute the application builder.
//        await _applicationBuilderExecutor.ExecuteBuildAsync(_builder, cancellationToken);

//        _logger.LogInformation("WebApplication built successfully.");

//        return CreateApplication(_builder);
//    }
//}
//}