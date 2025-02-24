// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Application;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using System.Reflection;

namespace FluentInjections.Injection
{
    public static class InjectionBuilder
    {
        public static IInjectionBuilder<TBuilder> For<TBuilder>(string[]? args = null, ILoggerFactory? loggerFactory = null, IServiceCollection? externalServices = null)
            where TBuilder : class, IInjectionBuilder<TBuilder>, IApplicationBuilder<TBuilder>
        {
            loggerFactory ??= LoggerFactory.Create(builder => builder.AddConsole());
            externalServices ??= new ServiceCollection();
            var builderRegistry = new BuilderRegistry();

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
                            var factoryType = typeof(IBuilderFactory<>).MakeGenericType(attribute.BuilderType);
                            var factoryInstance = Activator.CreateInstance(attribute.BuilderFactoryType);

                            if (!factoryType.IsInstanceOfType(factoryInstance))
                            {
                                logger.LogWarning("Builder factory type {FactoryType} does not implement IBuilderFactory<{BuilderType}>", attribute.BuilderFactoryType, attribute.BuilderType);
                                continue;
                            }

                            var factoryMethod = attribute.BuilderFactoryType.GetMethod("CreateBuilder")?
                                .MakeGenericMethod(attribute.BuilderType);

                            if (factoryMethod is null)
                            {
                                logger.LogWarning("Factory method for builder type {BuilderType} not found", attribute.BuilderType);
                                continue;
                            }

                            var factory = factoryMethod.Invoke(factoryInstance, new object[] { loggerFactory }) as Delegate;

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
            return builder;
        }
    }
}
