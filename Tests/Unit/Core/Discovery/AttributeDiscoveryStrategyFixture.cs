// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentAssertions;

using FluentInjections.Core.Abstractions;
using FluentInjections.Core.Discovery;
using FluentInjections.Extensions;

using Microsoft.Extensions.Logging;

using Moq;

using System.Reflection;

namespace FluentInjections.Tests.Units.Core.Discovery
{
    public class AttributeDiscoveryStrategyFixture
    {
        public Mock<ILogger<AttributeDiscoveryStrategy>> MockLogger { get; }
        public Mock<ILogger<AssemblyScanner>> MockAssemblyScannerLogger { get; }
        public Mock<ITypeDiscoveryContext> MockContext { get; }
        public Mock<IAssemblyFilter> MockFilter { get; }
        public Mock<AssemblyScanner> MockAssemblyScanner { get; }
        public AttributeDiscoveryStrategy Strategy { get; }
        public Mock<Assembly> MockAssemblyWithException { get; }
        public Mock<ILoggerFactory> MockLoggerFactory { get; }

        public AttributeDiscoveryStrategyFixture()
        {
            MockLogger = new Mock<ILogger<AttributeDiscoveryStrategy>>();
            MockAssemblyScannerLogger = new Mock<ILogger<AssemblyScanner>>();
            MockContext = new Mock<ITypeDiscoveryContext>();
            MockFilter = new Mock<IAssemblyFilter>();
            MockAssemblyScanner = new Mock<AssemblyScanner>(MockAssemblyScannerLogger.Object);
            MockAssemblyWithException = new Mock<Assembly>();
            MockLoggerFactory = new Mock<ILoggerFactory>();

            // Setup the non-generic CreateLogger to return the mock loggers.
            MockLoggerFactory
                .Setup(x => x.CreateLogger(typeof(AttributeDiscoveryStrategy).FullName ?? typeof(AttributeDiscoveryStrategy).Name))
                .Returns(MockLogger.Object);

            MockLoggerFactory
                .Setup(x => x.CreateLogger(typeof(AssemblyScanner).FullName ?? typeof(AssemblyScanner).Name))
                .Returns(MockAssemblyScannerLogger.Object);

            MockLogger.Setup(l => l.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()
            )).Verifiable();

            MockAssemblyScannerLogger.Setup(l => l.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()
            )).Verifiable();

            MockContext.SetupGet(c => c.AssemblyFilter).Returns(MockFilter.Object);

            var reflectionException = new ReflectionTypeLoadException(new Type[0], new[] { new TypeLoadException("Failed to load type") });
            MockAssemblyWithException.Setup(a => a.GetTypes()).Throws(reflectionException);

            var assemblies = new[] { Assembly.GetExecutingAssembly(), MockAssemblyWithException.Object };

            MockAssemblyScanner.Setup(s => s.ScanAssemblies(MockFilter.Object, It.IsAny<CancellationToken>()))
                .Returns(assemblies.ToAsyncEnumerable(CancellationToken.None));

            Strategy = new AttributeDiscoveryStrategy(MockLoggerFactory.Object, MockAssemblyScanner.Object);

            var testAssembly = typeof(TestClassWithAttribute).Assembly;
            var testAssembly2 = typeof(TestClassWithoutAttribute).Assembly;
            testAssembly.Should().BeSameAs(testAssembly2);
        }

        public void SetupFilterToThrowException()
        {
            MockFilter.Setup(f => f.ShouldInclude(It.IsAny<Assembly>())).Throws(new Exception("Test Exception"));
        }

        public void SetupFilterToReturnTrue()
        {
            MockFilter.Setup(f => f.ShouldInclude(It.IsAny<Assembly>())).Returns(true);
        }

        public void ResetFilter()
        {
            MockFilter.Reset();
        }

        public void SetupGenericLoggers()
        {
            MockLogger.Setup(l => l.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()
            )).Verifiable();

            MockAssemblyScannerLogger.Setup(l => l.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()
            )).Verifiable();
        }

        public void VerifyReflectionTypeLoadExceptionLogged(Assembly assemblyWithException)
        {
            MockLogger.Verify(l => l.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains($"Error loading types from assembly: {assemblyWithException.FullName}")),
                It.IsAny<ReflectionTypeLoadException>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!
            ), Times.Once);

            MockLogger.Verify(l => l.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Loader Exception")),
                It.IsAny<TypeLoadException>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!
            ), Times.Once);
        }

        public void ResetGenericLoggers()
        {
            MockLogger.Reset();
            MockAssemblyScannerLogger.Reset();
        }

        public void SetupContext(IEnumerable<Assembly> assemblies, Type interfaceType = null!, Type attributeType = null!)
        {
            MockContext.SetupGet(c => c.Assemblies).Returns(assemblies);
            MockContext.SetupGet(c => c.InterfaceType).Returns(interfaceType);
            MockContext.SetupGet(c => c.AttributeType).Returns(attributeType);
        }

        public void ResetContext()
        {
            MockContext.Reset();
        }
    }
}
