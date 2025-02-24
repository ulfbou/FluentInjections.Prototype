// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections.Configuration
{
    public class ConfigurationException : Exception
    {
        public ConfigurationException() { }

        public ConfigurationException(string? message) : base(message) { }

        public ConfigurationException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}
