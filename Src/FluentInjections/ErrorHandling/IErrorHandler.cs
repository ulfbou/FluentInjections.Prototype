// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.Logging;

using System.Diagnostics;
using System.Text.Json;
using OpenTelemetry.Trace;

namespace FluentInjections.ErrorHandling
{
    public class AppErrorHandler : IErrorHandler
    {
        private readonly ILogger<AppErrorHandler> _logger;

        public AppErrorHandler(ILogger<AppErrorHandler> logger)
        {
            _logger = logger;
        }

        public async Task HandleExceptionAsync(Exception ex, string context = null!, IDictionary<string, string> additionalData = null!)
        {
            var errorId = Guid.NewGuid();
            var errorDetails = new
            {
                ErrorId = errorId,
                Timestamp = DateTime.UtcNow,
                Context = context,
                ExceptionType = ex.GetType().FullName,
                Message = ex.Message,
                StackTrace = ex.StackTrace,
                AdditionalData = additionalData
            };

            var errorJson = JsonSerializer.Serialize(errorDetails);

            _logger.LogError(ex, $"Error ID: {errorId}. Context: {context}. Details: {errorJson}");

            // Integration with distributed tracing (OpenTelemetry)
            if (Activity.Current != null)
            {
                try
                {
                    // Use Activity.AddException
                    Activity.Current.AddException(ex);
                }
                catch (Exception addExceptionError)
                {
                    // If AddException fails, fall back to recording exception details as tags
                    _logger.LogWarning(addExceptionError, "Failed to record exception using Activity.Current.AddException. Falling back to tags.");

                    Activity.Current.SetTag("exception.type", ex.GetType().FullName);
                    Activity.Current.SetTag("exception.message", ex.Message);
                    Activity.Current.SetTag("exception.stacktrace", ex.StackTrace);
                }
            }

            // Integration with error reporting services (Application Insights, Sentry)
            // Example: _telemetryClient.TrackException(ex);

            await Task.CompletedTask; // Placeholder for asynchronous error reporting
        }
    }
    public interface IErrorHandler
    {
        Task HandleExceptionAsync(Exception ex, string context = null!, IDictionary<string, string> additionalData = null!);
    }
}
