// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Application;
using FluentInjections.Injection;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using System.Diagnostics;
using System.Reflection;

namespace FluentInjections;

public static class InjectionBuilder
{
    public static IInjectionBuilder<TBuilder> For<TBuilder>(string[]? args = null, ILoggerFactory? loggerFactory = null, IServiceCollection? externalServices = null)
        where TBuilder : class, IApplicationBuilder<TBuilder>
    {
        loggerFactory ??= LoggerFactory.Create(builder => builder.AddConsole());
        externalServices ??= new ServiceCollection();
        var composer = new ApplicationComposer(loggerFactory);
        var builderRegistry = new BuilderRegistry();

        // Register WebApplicationBuilderWrapper
        //builderRegistry.RegisterBuilder<WebApplicationBuilder>((args, services, loggerFactory) =>
        //    (WebApplicationBuilder)new WebApplicationBuilderFactory(loggerFactory!).CreateApplicationBuilder(args, services!));

        // Discover and register other builders (if any)
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        var logger = loggerFactory.CreateLogger("InjectionBuilder");

        foreach (var assembly in assemblies)
        {
            var builderTypes = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.GetCustomAttribute<BuilderAttribute>() != null);

            foreach (var builderType in builderTypes)
            {
                var attribute = builderType.GetCustomAttribute<BuilderAttribute>();

                if (attribute is not null)
                {
                    try
                    {
                        var factoryMethod = typeof(InjectionBuilder).GetMethod(nameof(CreateBuilderFactory), BindingFlags.Static | BindingFlags.NonPublic)?
                                            .MakeGenericMethod(attribute.BuilderType);

                        if (factoryMethod is null)
                        {
                            logger.LogWarning("Factory method for builder type {BuilderType} not found", attribute.BuilderType);
                            continue;
                        }

                        var factory = factoryMethod.Invoke(null, new object[] { loggerFactory }) as Delegate;

                        if (factory is null)
                        {
                            logger.LogWarning("Factory method for builder type {BuilderType} could not be created", attribute.BuilderType);
                            continue;
                        }

                        var registerMethod = typeof(BuilderRegistry).GetMethod(nameof(BuilderRegistry.RegisterBuilder));

                        if (registerMethod is null)
                        {
                            logger.LogWarning("Register method for builder type {BuilderType} not found", attribute.BuilderType);
                            continue;
                        }

                        var genericRegisterMethod = registerMethod.MakeGenericMethod(attribute.BuilderType);

                        if (genericRegisterMethod is null)
                        {
                            logger.LogWarning("Generic register method for builder type {BuilderType} not found", attribute.BuilderType);
                            continue;
                        }

                        genericRegisterMethod.Invoke(builderRegistry, new object[] { factory });
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Error registering builder type {BuilderType}", attribute.BuilderType);
                    }
                }
            }
        }

        var builder = builderRegistry.CreateBuilder<TBuilder>(args, externalServices, loggerFactory);
        return composer.CreateInjectionBuilder((TBuilder)builder);
    }

    private static Func<string[]?, IServiceCollection?, ILoggerFactory?, TBuilder> CreateBuilderFactory<TBuilder>(ILoggerFactory loggerFactory)
        where TBuilder : class, IApplicationBuilder<TBuilder>
    {
        return (args, services, lf) =>
        {
            return (TBuilder)Activator.CreateInstance(typeof(TBuilder), new object[] { services, lf });
        };
    }
}
