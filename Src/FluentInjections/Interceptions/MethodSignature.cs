// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections.Interceptions;

public record struct MethodSignature(Type DeclaringType, string MethodName, Type[] ParameterTypes)
{
    public bool Equals(MethodSignature other)
    {
        if (DeclaringType != other.DeclaringType || MethodName != other.MethodName)
            return false;
        if (ParameterTypes.Length != other.ParameterTypes.Length)
            return false;
        return !ParameterTypes.Where((t, i) => t != other.ParameterTypes[i]).Any();
    }

    public override int GetHashCode()
    {
        int hash = HashCode.Combine(DeclaringType, MethodName);
        foreach (var paramType in ParameterTypes)
        {
            hash = HashCode.Combine(hash, paramType);
        }
        return hash;
    }
}
