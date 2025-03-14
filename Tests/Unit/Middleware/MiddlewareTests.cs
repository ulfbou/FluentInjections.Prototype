// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Xunit.Abstractions;

namespace FluentInjections.Tests.Units.Middleware
{
    public class MiddlewareTests
    {
        private readonly ITestOutputHelper _outputHelper;

        public MiddlewareTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }
    }
}
