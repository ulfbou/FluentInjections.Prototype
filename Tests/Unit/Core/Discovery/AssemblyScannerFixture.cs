// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Moq;
using FluentInjections.Core.Abstractions;
using FluentInjections.Core.Discovery;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace FluentInjections.Tests.Units.Core.Discovery
{
    public class AssemblyScannerFixture
    {
        public Mock<ILogger<AssemblyScanner>> MockLogger { get; }
        public Mock<IAssemblyFilter> MockFilter { get; }
        public AssemblyScanner Scanner { get; }

        public AssemblyScannerFixture()
        {
            MockLogger = new Mock<ILogger<AssemblyScanner>>();
            MockFilter = new Mock<IAssemblyFilter>();
            Scanner = new AssemblyScanner(MockLogger.Object);
        }

        public void SetupFilterToReturnAssemblies(Assembly[] assembliesToReturn)
        {
            MockFilter.Setup(f => f.ShouldInclude(It.IsAny<Assembly>()))
                .Returns((Assembly assembly) => assembliesToReturn.Contains(assembly));
        }

        public void SetupFilterToThrowException()
        {
            MockFilter.Setup(f => f.ShouldInclude(It.IsAny<Assembly>())).Throws<Exception>();
        }

        public void SetupFilterToReturnFalse()
        {
            MockFilter.Setup(f => f.ShouldInclude(It.IsAny<Assembly>())).Returns(false);
        }

        public void ResetFilter()
        {
            MockFilter.Reset();
        }

        public void VerifyDebugLog(Assembly[] assemblies)
        {
            MockLogger.Verify(
                l => l.Log(
                    LogLevel.Debug,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Scanning assembly")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!
                ),
                Times.Exactly(assemblies.Length)
            );
        }

        public void VerifyErrorLog()
        {
            MockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Error loading assembly")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!
                ),
                Times.AtLeastOnce
            );
        }

        public void VerifyNoErrorLogs()
        {
            MockLogger.Verify(
                l => l.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!
                ),
                Times.Never
            );
        }

        public void VerifyLogger()
        {
            MockLogger.Verify();
        }

        public void ResetLogger()
        {
            MockLogger.Reset();
        }
    }
}
