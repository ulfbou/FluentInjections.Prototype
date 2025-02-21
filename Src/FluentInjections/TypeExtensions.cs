// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections.Extensions;

public static class TypeExtensions
{
    public static bool ImplementsGenericInterface(this Type type, Type genericType, Type constraint)
    {
        return type.GetInterfaces()
                   .Concat(new[] { type })
                   .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericType && constraint.IsAssignableFrom(i.GetGenericArguments()[0]));
    }
}
