// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Core.Abstractions;
using FluentInjections.Core.Discovery;
using FluentInjections.Extensions;
using FluentInjections.Utilities.Collections;

using Microsoft.Extensions.Logging;

using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace FluentInjections.Core.Discovery
{
    public class AttributeDiscoveryStrategy : ITypeDiscoveryStrategy
    {
        private readonly ILoggerFactory _loggerFactory;
        private AssemblyScanner? _scanner;

        public AttributeDiscoveryStrategy(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }

        internal AttributeDiscoveryStrategy(ILoggerFactory loggerFactory, AssemblyScanner scanner) : this(loggerFactory)
        {
            _scanner = scanner;
        }

        public async IAsyncEnumerable<Type> Discover(ITypeDiscoveryContext context, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var logger = _loggerFactory.CreateLogger<AttributeDiscoveryStrategy>();
            var assemblies = await ScanAssemblies(context, cancellationToken);

            if (assemblies is null)
            {
                yield break;
            }

            await foreach (var assembly in assemblies)
            {
                var types = GetTypesFromAssembly(assembly, context, logger);

                if (types is null)
                {
                    continue;
                }

                foreach (var type in types)
                {
                    if (ShouldYieldType(type, context))
                    {
                        logger.LogDebug("Discovered type: {TypeName}", type.FullName ?? type.Name);
                        yield return type;
                    }
                }
            }
        }

        private async Task<IAsyncEnumerable<Assembly>?> ScanAssemblies(ITypeDiscoveryContext context, CancellationToken cancellationToken)
        {
            try
            {
                var scanner = _scanner ?? new AssemblyScanner(_loggerFactory.CreateLogger<AssemblyScanner>());
                var result = scanner.ScanAssemblies(context.AssemblyFilter, cancellationToken);

                var logger = _loggerFactory.CreateLogger<AttributeDiscoveryStrategy>(); // or use the AssemblyScanner logger.

                if (result != null)
                {
                    IEnumerable<Assembly> assemblies = await result.ToEnumerable(cancellationToken: cancellationToken); //convert to enumerable for inspection.
                    foreach (var assembly in assemblies)
                    {
                        logger.LogDebug($"Scanning assembly: {assembly.FullName}");
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                _loggerFactory.CreateLogger<AttributeDiscoveryStrategy>().LogError(ex, "Error scanning assemblies");
                return null;
            }
        }

        private IEnumerable<Type>? GetTypesFromAssembly(Assembly assembly, ITypeDiscoveryContext context, ILogger<AttributeDiscoveryStrategy> logger)
        {
            try
            {
                var types = assembly.GetTypes();

                // Add logging here to inspect the types
                foreach (var type in types)
                {
                    logger.LogDebug($"Loaded type: {type.FullName} from assembly: {assembly.FullName}");
                }

                return types;
            }
            catch (ReflectionTypeLoadException ex)
            {
                logger.LogError(ex, $"Error loading types from assembly: {assembly.FullName}");

                foreach (var loaderException in ex.LoaderExceptions)
                {
                    logger.LogError(loaderException, "Loader Exception");
                }

                return null;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Unexpected error loading types from assembly: {assembly.FullName}");
                return null;
            }
        }

        private bool ShouldYieldType(Type type, ITypeDiscoveryContext context)
        {
            if (type.IsInterface || type.IsAbstract) return false;
            if (context.InterfaceType != null && !context.InterfaceType.IsAssignableFrom(type)) return false;
            if (context.AttributeType != null && type.GetCustomAttribute(context.AttributeType) == null) return false;
            return true;
        }
    }
}
