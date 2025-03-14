// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Core.Abstractions;

using Microsoft.Extensions.Logging;

using System.Reflection;
using System.Runtime.CompilerServices;

namespace FluentInjections.Core.Discovery
{
    public class AssemblyScanner
    {
        private readonly ILogger<AssemblyScanner> _logger;

        public AssemblyScanner(ILogger<AssemblyScanner> logger)
        {
            _logger = logger;
        }

        public virtual async IAsyncEnumerable<Assembly> ScanAssemblies(IAssemblyFilter filter, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                cancellationToken.ThrowIfCancellationRequested();

                try
                {
                    if (!filter.ShouldInclude(assembly))
                    {
                        continue;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error loading assembly {AssemblyName}", assembly.FullName);
                }

                _logger.LogDebug("Scanning assembly {AssemblyName}", assembly.FullName);
                yield return assembly;
            }

            await Task.CompletedTask;
        }
    }
}
