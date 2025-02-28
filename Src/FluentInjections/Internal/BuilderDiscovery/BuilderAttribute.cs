// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions;
using FluentInjections.Abstractions.Factories;

namespace FluentInjections.Internal.BuilderDiscovery
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class BuilderAttribute : Attribute
    {
        public Type BuilderType { get; }
        public Type BuilderFactoryType { get; }
        public bool IsValid => _errors.Count == 0;
        public IEnumerable<string> Errors => _errors;
        private readonly List<string> _errors = new List<string>();

        public BuilderAttribute(Type builderType, Type builderFactoryType)
        {
            if (builderFactoryType == null)
            {
                _errors.Add("BuilderFactoryType cannot be null");
            }

            if (builderType == null)
            {
                _errors.Add("BuilderType cannot be null");
            }
            else if (!builderType.IsInterface)
            {
                _errors.Add("BuilderType must be an interface.");
            }
            else if (!typeof(IBuilderFactory<>).MakeGenericType(builderType).IsAssignableFrom(builderFactoryType))
            {
                _errors.Add($"BuilderFactoryType must implement IBuilderFactory<{builderType.Name}>.");
            }

            BuilderType = builderType!;
            BuilderFactoryType = builderFactoryType!;
        }
    }
}
