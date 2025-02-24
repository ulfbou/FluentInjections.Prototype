// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Validation;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace FluentInjections.Configuration
{
    public class FrameworkConfiguration : IFrameworkConfiguration, IConfiguration
    {
        private readonly IConfiguration _configuration;

        public FrameworkConfiguration(IConfiguration configuration)
        {
            Guard.NotNull(configuration, nameof(configuration));
            _configuration = configuration;
        }

        public string? this[string key] { get => _configuration[key]; set => _configuration[key] = value; }

        public LogLevel LogLevel => _configuration.GetValue<LogLevel>("LogLevel");
        public int AsyncTimeoutMilliseconds => _configuration.GetValue<int>("AsyncTimeoutMilliseconds");
        public string ConnectionString => _configuration.GetValue<string>("ConnectionString") ?? throw new ConfigurationException("ConnectionString is required.");
        public bool EnableTelemetry => _configuration.GetValue<bool>("EnableTelemetry");
        public string TelemetryEndpoint => _configuration.GetValue<string>("TelemetryEndpoint") ?? throw new ConfigurationException("TelemetryEndpoint is required.");
        public string EnvironmentName => _configuration.GetValue<string>("EnvironmentName") ?? throw new ConfigurationException("EnvironmentName is required.");

        public IEnumerable<IConfigurationSection> GetChildren() => _configuration.GetChildren();
        public IChangeToken GetReloadToken() => _configuration.GetReloadToken();
        public IConfigurationSection GetSection(string key) => _configuration.GetSection(key);
    }
}
