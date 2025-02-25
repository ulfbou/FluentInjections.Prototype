// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Configuration;
using FluentInjections.ErrorHandling;

using Microsoft.Extensions.Logging;

namespace FluentInjections.Async
{
    public class DefaultAsyncOperationManager : IAsyncOperationManager
    {
        private readonly ILogger<DefaultAsyncOperationManager> _logger;
        private readonly IErrorHandler _errorHandler;
        private readonly IFrameworkConfiguration _config;

        public DefaultAsyncOperationManager(ILogger<DefaultAsyncOperationManager> logger, IErrorHandler errorHandler, IFrameworkConfiguration config)
        {
            _logger = logger;
            _errorHandler = errorHandler;
            _config = config;
        }

        public async ValueTask<T> ExecuteAsync<T>(Func<CancellationToken, ValueTask<T>> taskFunction, int timeoutMilliseconds = Timeout.Infinite)
        {
            using (var cts = new CancellationTokenSource())
            {
                var combinedCts = CancellationTokenSource.CreateLinkedTokenSource(cts.Token);
                if (timeoutMilliseconds != Timeout.Infinite)
                {
                    cts.CancelAfter(_config.AsyncTimeoutMilliseconds > 0 ? _config.AsyncTimeoutMilliseconds : timeoutMilliseconds); // Configurable Timeout
                }

                try
                {
                    return await taskFunction(combinedCts.Token).ConfigureAwait(false); // ConfigureAwait(false) by default
                }
                catch (OperationCanceledException ex)
                {
                    if (cts.IsCancellationRequested && !combinedCts.IsCancellationRequested)
                    {
                        await _errorHandler.HandleExceptionAsync(new TimeoutException("The operation timed out.", ex), "Async Operation").ConfigureAwait(false);
                        throw new TimeoutException("The operation timed out.", ex);
                    }
                    else
                    {
                        _logger.LogDebug("Operation cancelled externally.");
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    await _errorHandler.HandleExceptionAsync(ex, "Async Operation").ConfigureAwait(false);
                    throw;
                }
            }
        }

        public async ValueTask ExecuteAsync(Func<CancellationToken, ValueTask> taskFunction, int timeoutMilliseconds = Timeout.Infinite)
        {
            await ExecuteAsync<bool>(async ct => { await taskFunction(ct); return true; }, timeoutMilliseconds);
        }
    }
}
