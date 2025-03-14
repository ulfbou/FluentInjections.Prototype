// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Concurrent;

using FluentInjections.Abstractions.Adapters;

namespace FluentInjections.Adapters.AspNetCore
{
    public class ApplicationTypeRegistry : IApplicationTypeRegistry
    {
        public static string FrameworkName { get; } = "ASP.NET Core";

        private readonly ConcurrentDictionary<Type, Func<object>> _factories = new();
        private readonly List<IApplicationTypeMetadata> _metadata = new();

        public IReadOnlyCollection<IApplicationTypeMetadata> RegisteredApplications => _metadata.AsReadOnly();

        public void Register<TAdapter, TInnerApplication, TBuilder>(Func<TBuilder> factory)
            where TAdapter : IApplicationAdapter<TInnerApplication>
            where TBuilder : IApplicationBuilderAdapter<TInnerApplication>
        {
            _factories[typeof(TInnerApplication)] = () => factory();
            _metadata.Add(new ApplicationTypeMetadata(typeof(TInnerApplication), typeof(TBuilder), FrameworkName));
        }
    }
}
