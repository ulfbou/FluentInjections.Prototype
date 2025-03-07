// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.Logging;

namespace FluentInjections.Abstractions.Factories
{
    internal class LoggerFactoryAdapter : ILoggerProvider
    {
        private ILoggerFactory loggerFactory;

        public LoggerFactoryAdapter(ILoggerFactory loggerFactory) => this.loggerFactory = loggerFactory;

        public ILogger CreateLogger(string categoryName)
        {
            return loggerFactory.CreateLogger(categoryName);
        }

        public void Dispose()
        {
            loggerFactory.Dispose();
        }
    }
}