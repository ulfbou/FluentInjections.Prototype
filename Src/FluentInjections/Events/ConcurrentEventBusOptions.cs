// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections.Events;

public class ConcurrentEventBusOptions
{
    public int MaxConcurrentHandlers { get; set; } = 8;
    public int MaxConcurrentEvents { get; set; } = 4;
}
