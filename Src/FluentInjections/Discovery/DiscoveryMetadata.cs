// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections.Discovery
{
    public class DiscoveryMetadata<TItem>
    {
        public required string Name { get; set; }
        public required string Version { get; set; }
        public List<string> Dependencies { get; set; } = new List<string>();
        public int Priority { get; set; }
        public Dictionary<string, object> CustomProperties { get; set; } = new Dictionary<string, object>();
        public required Type ItemType { get; set; }
    }
}
