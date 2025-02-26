// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions;

using Microsoft.Extensions.Logging;

namespace FluentInjections.Logging
{
    public interface ILoggerFactoryProvider
    {
        ILoggerFactory GetLoggerFactory();
    }
    internal class NullLoggerFactoryProvider : ILoggerFactoryProvider
    {
        public static NullLoggerFactoryProvider Instance { get; } = new NullLoggerFactoryProvider();
        public ILoggerFactory GetLoggerFactory() => NullLoggerFactory.Instance;
    }
    internal class NullLoggerFactory : ILoggerFactory
    {
        public static NullLoggerFactory Instance { get; } = new NullLoggerFactory();
        public void Dispose() { }
        public ILogger CreateLogger(string categoryName) => NullLogger.Instance;
        public void AddProvider(ILoggerProvider provider) { }
    }
    internal class NullLogger : ILogger
    {
        public static NullLogger Instance { get; } = new NullLogger();
        IDisposable? ILogger.BeginScope<TState>(TState state) => NullScope.Instance;
        public bool IsEnabled(LogLevel logLevel) => false;
        void ILogger.Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) { }
    }
    internal class NullScope : IDisposable
    {
        public static NullScope Instance { get; } = new NullScope();
        public void Dispose() { }
    }
}
