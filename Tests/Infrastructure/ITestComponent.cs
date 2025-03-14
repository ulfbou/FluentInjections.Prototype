// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions;

namespace FluentInjections.Tests.Infrastructure
{
    public interface ITestComponent : IComponent { }
    public interface ITestConfigurator : IConfigurator<ITestComponent> { }
}
