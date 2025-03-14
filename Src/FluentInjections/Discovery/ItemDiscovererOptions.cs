// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections.Discovery
{
    public class ItemDiscovererOptions
    {
        public int CacheExpirationMinutes { get; set; } = 10;
        public string CacheKeyPrefix { get; set; } = "DiscoveredItems_";
    }
}
