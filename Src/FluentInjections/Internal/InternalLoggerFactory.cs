// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.Logging;

using System.Collections.Concurrent;
using System.Text;

namespace FluentInjections.Internal
{
    internal class InternalLoggerFactory : ILoggerFactory
    {
        private readonly ConcurrentBag<ILoggerProvider> _providers = new();
        private readonly ConcurrentDictionary<string, InternalLogger> _loggers = new();
        private readonly LogLevel _minLogLevel;

        public InternalLoggerFactory(LogLevel minLogLevel = LogLevel.Information)
        {
            _minLogLevel = minLogLevel;
        }

        public void AddProvider(ILoggerProvider provider)
        {
            _providers.Add(provider);
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, name => new InternalLogger(name, _minLogLevel));
        }

        public void Dispose()
        {
            _loggers.Clear();

            foreach (var provider in _providers)
            {
                provider.Dispose();
            }
        }
        internal class InternalLogger : ILogger
        {
            private readonly string _categoryName;
            private readonly LogLevel _minLogLevel;

            public InternalLogger(string categoryName, LogLevel minLogLevel)
            {
                _categoryName = categoryName;
                _minLogLevel = minLogLevel;
            }

            public IDisposable BeginScope<TState>(TState state) where TState : notnull
            {
                return new LoggerScope();
            }

            public bool IsEnabled(LogLevel logLevel) => logLevel >= _minLogLevel;

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
            {
                if (!IsEnabled(logLevel))
                {
                    return;
                }

                var logRecord = new StringBuilder($"{DateTime.UtcNow:o} [{logLevel}] {_categoryName} - {formatter(state, exception)}");

                if (exception != null)
                {
                    logRecord.AppendLine().Append(exception);
                }

                Console.WriteLine(logRecord.ToString());
            }

            private class LoggerScope : IDisposable
            {
                public void Dispose() { }
            }
        }
    }
}
