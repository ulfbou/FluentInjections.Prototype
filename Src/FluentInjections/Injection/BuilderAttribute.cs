// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections.Injection
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class BuilderAttribute : Attribute
    {
        public Type BuilderType { get; }
        public Type BuilderFactoryType { get; }

        public BuilderAttribute(Type builderType, Type builderFactoryType)
        {
            if (!typeof(IBuilderFactory<>).MakeGenericType(builderType).IsAssignableFrom(builderFactoryType))
            {
                throw new ArgumentException($"BuilderFactoryType must implement IBuilderFactory<{builderType.Name}>.");
            }
            BuilderType = builderType;
            BuilderFactoryType = builderFactoryType;
        }
    }
}
