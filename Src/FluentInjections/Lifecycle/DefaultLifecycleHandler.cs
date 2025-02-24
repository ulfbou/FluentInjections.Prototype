// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections.Lifecycle
{
    public class DefaultLifecycleHandler : ILifecycleHandler
    {
        public virtual async Task StartAsync(CancellationToken cancellationToken)
        {
            // Implementation for starting lifecycle components
            await Task.CompletedTask;
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            // Implementation for stopping lifecycle components
            await Task.CompletedTask;
        }
    }
}
