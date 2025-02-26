// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Builder;

namespace FluentInjections.Abstractions
{
    public interface IConcreteApplicationAdapter<TConcreteApplication> : IApplicationAdapter
    {
        TConcreteApplication ConcreteApplication { get; }
        IBuilderAdapter<WebApplication> Adapter { get; }
    }
}
