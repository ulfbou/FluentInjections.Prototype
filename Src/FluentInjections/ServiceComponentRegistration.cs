// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Validation;

namespace FluentInjections;

internal class ServiceRegistration<TContract>
    : ComponentRegistration<IServiceComponent, TContract>
    , IComponentRegistration<IServiceComponent, TContract>
{ }
