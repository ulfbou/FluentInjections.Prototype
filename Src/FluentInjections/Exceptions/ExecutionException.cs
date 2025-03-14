// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections.Exceptions
{
    public class ExecutionException : Exception
    {
        public ExecutionException(string message) : base(message) { }
        public ExecutionException(string message, Exception innerException) : base(message, innerException) { }
    }
}
