// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Builder;

namespace FluentInjections.Abstractions.Adapters
{
    public interface IConcreteApplicationAdapter<TConcreteApplication> : IApplicationAdapter
            where TConcreteApplication : notnull
    {
        TConcreteApplication ConcreteApplication { get; }
        IBuilderAdapter<WebApplication> Adapter { get; }
    }
}
