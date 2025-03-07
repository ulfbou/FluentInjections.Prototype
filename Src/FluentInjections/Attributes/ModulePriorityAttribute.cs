// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ModulePriorityAttribute : Attribute
    {
        public ModulePriorityAttribute(int priority)
        {
            Priority = priority;
        }
        public int Priority { get; }
    }
}