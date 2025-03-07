// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class ModuleDependencyAttribute : Attribute
    {
        public ModuleDependencyAttribute(Type dependencyType)
        {
            DependencyType = dependencyType;
        }
        public Type DependencyType { get; }
    }
}