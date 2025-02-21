// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.Options;

namespace FluentInjections.Events;

public class ConfigureConcurrentEventBusOptions : IConfigureOptions<ConcurrentEventBusOptions>
{
    public void Configure(ConcurrentEventBusOptions options)
    {
        options.MaxConcurrentEvents = 6;
        options.MaxConcurrentHandlers = 12;
    }
}
