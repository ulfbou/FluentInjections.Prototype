// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections.Exceptions
{
    public class MissingDependencyException : Exception
    {
        public MissingDependencyException(string message) : base(message) { }
    }
}
