// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Adapters;

namespace FluentInjections.Abstractions.AspNetCore
{
    internal class CoreWebAppBuilderAbstraction : ICoreWebAppBuilderAbstraction
    {
        public Type AdapterFactoryType => typeof(DefaultApplicationAdapterFactory);
    }
}
