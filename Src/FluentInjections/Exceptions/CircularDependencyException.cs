// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections.Exceptions
{
    public class CircularDependencyException : Exception
    {
        public CircularDependencyException(string message) : base(message) { }
    }
}
