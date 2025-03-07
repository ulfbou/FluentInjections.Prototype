// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.


namespace FluentInjections.Exceptions
{
    [Serializable]
    internal class ComponentResolutionException : Exception
    {
        public ComponentResolutionException()
        {
        }

        public ComponentResolutionException(string? message) : base(message)
        {
        }

        public ComponentResolutionException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}