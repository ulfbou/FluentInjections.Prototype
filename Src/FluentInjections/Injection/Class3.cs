// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Application;
using FluentInjections.Collections;
using FluentInjections.Configurators;
using FluentInjections.Modules;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FluentInjections.Injection
{
    public class InjectionBuilder<TBuilder> : IInjectionBuilder<TBuilder>
            where TBuilder : IInjectionBuilder<TBuilder>
    {
        public IServiceCollection Services { get; } = new ServiceCollection();
        public TBuilder Builder => (TBuilder)(object)this;
        private IHostBuilder _hostBuilder;
        private bool _disposed = false;
        private string[] _arguments;
        private AssemblyCollection _servicesAssemblies;
        private AssemblyCollection _lifecyclesAssemblies;
        private AssemblyCollection _middlewareAssemblies;
        private Action<IApplicationBuilder<TBuilder>> _configureApp;
        private Action<AssemblyCollection> _serviceConfigurations;
        private Action<AssemblyCollection> _lifecycleConfigurations;
        private Action<AssemblyCollection> _middlewareConfigurations;
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger<InjectionBuilder<TBuilder>> _logger;

        public InjectionBuilder(ILoggerFactory loggerFactory, string[] arguments = null)
        {
            _loggerFactory = loggerFactory;
            _logger = loggerFactory.CreateLogger<InjectionBuilder<TBuilder>>();
            _arguments = arguments;
            _hostBuilder = Host.CreateDefaultBuilder();
            var logger = _loggerFactory.CreateLogger<AssemblyCollection>();
            _servicesAssemblies = new AssemblyCollection(logger);
            _lifecyclesAssemblies = new AssemblyCollection(logger);
            _middlewareAssemblies = new AssemblyCollection(logger);
            _configureApp = _ => { };
            _serviceConfigurations = _ => { };
            _lifecycleConfigurations = _ => { };
            _middlewareConfigurations = _ => { };
        }

        public IApplication<TBuilder>? Application { get; }

        public IInjectionBuilder<TBuilder> WithServices(Action<AssemblyCollection> configure)
        {
            _serviceConfigurations = configure;
            return this;
        }

        public IInjectionBuilder<TBuilder> WithLifecycles(Action<AssemblyCollection> configure)
        {
            _lifecycleConfigurations = configure;
            return this;
        }

        public IInjectionBuilder<TBuilder> WithMiddlewares(Action<AssemblyCollection> configure)
        {
            _middlewareConfigurations = configure;
            return this;
        }

        public IInjectionBuilder<TBuilder> WithServiceCollection(IServiceCollection services)
        {
            foreach (var descriptor in services)
            {
                Services.Add(descriptor);
            }
            return this;
        }

        public IInjectionBuilder<TBuilder> WithServiceProvider(IServiceProvider serviceProvider)
        {
            return this;
        }

        public IInjectionBuilder<TBuilder> WithArguments(string[] args)
        {
            _arguments = args;
            return this;
        }

        public Task<IApplication<TBuilder>> BuildAsync(CancellationToken cancellationToken = default)
        {
            var logger = _loggerFactory.CreateLogger<AssemblyCollection>();
            var assemblyCollection = new AssemblyCollection(logger);
            _serviceConfigurations?.Invoke(assemblyCollection);
            _lifecycleConfigurations?.Invoke(assemblyCollection);
            _middlewareConfigurations?.Invoke(assemblyCollection);

            foreach (var assembly in assemblyCollection)
            {
                var types = assembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract);

                foreach (var type in types)
                {
                    if (typeof(Modules.IModule).IsAssignableFrom(type))
                    {
                        Services.AddSingleton(typeof(IModule), Activator.CreateInstance(type));
                    }
                    else if (typeof(Configurators.IConfigurator).IsAssignableFrom(type))
                    {
                        Services.AddTransient(typeof(IConfigurator), type);
                    }
                    else
                    {
                        var configurableModuleInterface = type.GetInterfaces()
                            .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IConfigurableModule<>));

                        if (configurableModuleInterface != null)
                        {
                            Services.AddTransient(configurableModuleInterface, type);
                        }
                    }
                }
            }

            _hostBuilder.ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureServices(services =>
                {
                    foreach (var serviceDescriptor in Services)
                    {
                        services.Add(serviceDescriptor);
                    }
                });

                webBuilder.Configure(app =>
                {
                    _configureApp?.Invoke(app);
                });
            });

            var host = _hostBuilder.Build();
            return Task.FromResult<IApplication<TBuilder>>(new FluentApplication<TBuilder>(Builder, host));
        }

        public TInnerBuilder GetInnerBuilder<TInnerBuilder>()
        {
            return (TInnerBuilder)(object)_hostBuilder;
        }

        public TBuilder AddConfigurationSource(IConfigurationSource configurationSource)
        {
            _hostBuilder.ConfigureAppConfiguration(config =>
            {
                config.Add(configurationSource);
            });
            return Builder;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
            }
        }
    }
}
