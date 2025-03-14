// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections.Discovery
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class DependsOnAttribute : Attribute
    {
        public string DependencyName { get; }
        public DependsOnAttribute(string dependencyName)
        {
            DependencyName = dependencyName;
        }
    }
}
