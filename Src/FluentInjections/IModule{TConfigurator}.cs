// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.DependencyInjection;

using System.Reflection;

namespace FluentInjections;

public interface IModule<TConfigurator> : IModule where TConfigurator : IConfigurator { }
