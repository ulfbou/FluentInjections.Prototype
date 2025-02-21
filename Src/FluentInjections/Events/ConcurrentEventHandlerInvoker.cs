// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.Logging;

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace FluentInjections.Events;

/// <summary>
/// Invokes event handlers with timeout and cancellation support.
/// </summary>
public class ConcurrentEventHandlerInvoker : IConcurrentEventHandlerInvoker
{
    private readonly ILogger<ConcurrentEventHandlerInvoker> _logger;

    public ConcurrentEventHandlerInvoker(ILogger<ConcurrentEventHandlerInvoker> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Invokes the specified event handler with timeout and cancellation support.
    /// </summary>
    public async Task InvokeHandlerAsync<TEvent>(Func<TEvent, ValueTask> handler, TEvent @event, TimeSpan timeout, CancellationToken cancellationToken)
    {
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(timeout);

        try
        {
            var stopwatch = Stopwatch.StartNew();
            await handler(@event);
            stopwatch.Stop();
            _logger.LogInformation("Event handler for {EventType} executed in {ElapsedMilliseconds} ms", typeof(TEvent), stopwatch.ElapsedMilliseconds);
        }
        catch (OperationCanceledException) when (cts.IsCancellationRequested)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                _logger.LogWarning("Event handler for {EventType} was cancelled by caller.", typeof(TEvent));
            }
            else
            {
                _logger.LogWarning("Event handler for {EventType} timed out.", typeof(TEvent));
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling event of type {EventType}. Event Data: {EventData}", typeof(TEvent), GetEventDataSummary(@event));
        }
    }

    // Helper function to get a summary of event data (adjust as needed)
    private string GetEventDataSummary<TEvent>(TEvent @event)
    {
        if (@event == null) return "null";

        try
        {
            // Example:  Return a few key properties.  Customize this based on your events.
            if (@event is string str) return str.Length > 50 ? str.Substring(0, 50) + "..." : str;

            var properties = @event.GetType().GetProperties();
            var summary = new System.Text.StringBuilder();

            foreach (var prop in properties)
            {
                var value = prop.Name.Contains("Password") ? "********" : prop.GetValue(@event);
                summary.Append($"{prop.Name}: {value}, ");
            }

            return summary.ToString().TrimEnd(',', ' ');
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to create event data summary for type {EventType}", typeof(TEvent));
            return "Error creating summary";
        }
    }
}
