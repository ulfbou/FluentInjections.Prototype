// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Logging;

using IConfigurationProvider = FluentInjections.Configuration.IConfigurationProvider;

namespace FluentInjections.Abstractions.Adapters
{
    public interface IConcreteBuilderAdapter<TConcreteBuilder, TConcreteApplication>
            where TConcreteBuilder : notnull
            where TConcreteApplication : notnull
    {
        TConcreteBuilder ConcreteBuilder { get; }
        IConfigurationProvider ConfigurationProvider { get; }
        ILoggerFactoryProvider LoggerFactoryProvider { get; }
    }
}
