// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentAssertions;

using Moq;

using System.Reflection;

namespace FluentInjections.Tests.Units.Core.Discovery
{
    public class AssemblyScannerTests : IClassFixture<AssemblyScannerFixture>
    {
        private readonly AssemblyScannerFixture _fixture;

        public AssemblyScannerTests(AssemblyScannerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task ScanAssemblies_ShouldReturnAssemblies_WhenScanSucceeds()
        {
            // Arrange
            var assembliesToReturn = new[] { Assembly.GetExecutingAssembly(), typeof(string).Assembly };
            _fixture.SetupFilterToReturnAssemblies(assembliesToReturn);

            // Act
            var result = await _fixture.Scanner.ScanAssemblies(_fixture.MockFilter.Object, CancellationToken.None).ToListAsync();

            // Assert
            result.Should().HaveCount(assembliesToReturn.Length);
            result.Select(a => a.FullName).Should().BeEquivalentTo(assembliesToReturn.Select(a => a.FullName));

            _fixture.VerifyDebugLog(assembliesToReturn);
            _fixture.VerifyNoErrorLogs();
            _fixture.VerifyLogger();
            _fixture.ResetLogger();
            _fixture.ResetFilter();

        }

        [Fact]
        public async Task ScanAssemblies_ShouldReturnFilteredAssemblies()
        {
            // Arrange
            _fixture.MockFilter.Setup(f => f.ShouldInclude(It.IsAny<Assembly>())).Returns(true);

            // Act
            var assemblies = await _fixture.Scanner.ScanAssemblies(_fixture.MockFilter.Object).ToListAsync();

            // Assert
            assemblies.Should().NotBeEmpty();
            _fixture.VerifyLogger();
            _fixture.ResetLogger();
            _fixture.ResetFilter();
        }

        [Fact]
        public async Task ScanAssemblies_ShouldLogError_WhenExceptionOccurs()
        {
            // Arrange
            _fixture.SetupFilterToThrowException();

            // Act
            var assemblies = await _fixture.Scanner.ScanAssemblies(_fixture.MockFilter.Object).ToListAsync();

            // Assert
            _fixture.VerifyErrorLog();
            _fixture.VerifyLogger();
            _fixture.ResetLogger();
            _fixture.ResetFilter();
        }

        [Fact]
        public async Task ScanAssemblies_ShouldNotReturnAssembly_WhenFilterReturnsFalse()
        {
            // Arrange
            _fixture.SetupFilterToReturnFalse();

            // Act
            var assemblies = await _fixture.Scanner.ScanAssemblies(_fixture.MockFilter.Object).ToListAsync();

            // Assert
            assemblies.Should().BeEmpty();
            _fixture.VerifyLogger();
            _fixture.ResetLogger();
            _fixture.ResetFilter();
        }

        [Fact]
        public async Task ScanAssemblies_ShouldHandleCancellation()
        {
            // Arrange
            _fixture.MockFilter.Setup(f => f.ShouldInclude(It.IsAny<Assembly>())).Returns(true);
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            // Act
            try
            {
                await foreach (var _ in _fixture.Scanner.ScanAssemblies(_fixture.MockFilter.Object, cancellationTokenSource.Token))
                {
                    // The enumeration should be canceled before reaching this point.
                }
                Assert.Fail("The enumeration was not canceled."); // If it gets here, the enumeration was not cancelled.
            }
            catch (OperationCanceledException)
            {
                // Assert
                cancellationTokenSource.Token.IsCancellationRequested.Should().BeTrue();
            }

            _fixture.VerifyLogger();
            _fixture.ResetLogger();
            _fixture.ResetFilter();
        }

        [Fact]
        public async Task ScanAssemblies_ShouldHandleNullFilter()
        {
            // Arrange

            // Act
            var assemblies = await _fixture.Scanner.ScanAssemblies(null!).ToListAsync();

            // Assert
            assemblies.Should().NotBeEmpty(); // When null filter, all assemblies are returned.
            _fixture.VerifyLogger();
            _fixture.ResetLogger();
            _fixture.ResetFilter();
        }
    }
}
