// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Components;

using Microsoft.Extensions.DependencyInjection;

namespace FluentInjections.Lifecycle
{
    internal class LifecycleBuilder<TContract> : FluentComponentBuilder<ILifecycleComponent, TContract>, ILifecycleBuilder<TContract>
    {
        public LifecycleBuilder(IServiceCollection services, string alias) : base(services, alias) { }
    }
}
