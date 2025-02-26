// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace FluentInjections.Configuration
{
    internal sealed class NullConfigurationProvider : IConfigurationProvider
    {
        public static IConfigurationProvider Instance { get; } = new NullConfigurationProvider();

        public Microsoft.Extensions.Configuration.IConfiguration GetConfiguration() => Instance.GetConfiguration();
    }

    internal sealed class NullConfiguration : IConfiguration
    {
        public static IConfiguration Instance { get; } = new NullConfiguration();
        public string? this[string key] { get => default; set { } }

        public IEnumerable<IConfigurationSection> GetChildren() => Enumerable.Empty<IConfigurationSection>();
        public IChangeToken GetReloadToken() => NullChangeToken.Instance;
        public IConfigurationSection GetSection(string key) => Instance.GetSection(key);
    }
    internal sealed class NullChangeToken : IChangeToken
    {
        public static IChangeToken Instance { get; } = new NullChangeToken();
        public bool HasChanged => false;
        public bool ActiveChangeCallbacks => false;
        public IDisposable RegisterChangeCallback(Action<object?> callback, object? state) => NullDisposable.Instance;
    }
    internal sealed class NullDisposable : IDisposable
    {
        public static IDisposable Instance { get; } = new NullDisposable();
        public void Dispose() { }
    }
}
