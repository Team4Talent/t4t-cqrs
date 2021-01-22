using System.Reflection;
using System.Security.Claims;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using T4T.CQRS.Commands;
using T4T.CQRS.Extensions;
using T4T.CQRS.Tests.Commands;
using T4T.CQRS.Tests.Fakes;
using Xunit;

namespace T4T.CQRS.Tests.Extensions
{
    public class ICommandHandlerExtensionsTests
    {
        private readonly ICommandHandler<FakeCommand> _innerCommandHandler =
            new Mock<ICommandHandler<FakeCommand>>().Object;

        [Fact]
        public void WithExceptionHandling_wraps_the_innerCommandHandler_with_ExceptionHandlingCommandHandler()
        {
            var sut = _innerCommandHandler.WithExceptionHandling();
            Assert.IsType<ExceptionHandlingCommandHandler<FakeCommand>>(sut);
        }

        [Fact]
        public void WithAnyOfTheseClaims_wraps_the_innerCommandHandler_with_WithAnyOfTheseClaimsCommandHandler()
        {
            var sut = _innerCommandHandler.WithAnyOfTheseClaims(new ClaimsPrincipal());
            Assert.IsType<WithAnyOfTheseClaimsCommandHandler<FakeCommand>>(sut);
        }

        [Fact]
        public void WithRequiredClaim_wraps_the_innerCommandHandler_with_WithRequiredClaimCommandHandler()
        {
            var sut = _innerCommandHandler.WithRequiredClaim(new Claim("", ""), new ClaimsPrincipal());
            Assert.IsType<WithRequiredClaimCommandHandler<FakeCommand>>(sut);
        }

        [Fact]
        public void WithLogging_wraps_the_innerCommandHandler_with_WithLoggingCommandHandler()
        {
            var sut = _innerCommandHandler.WithLogging(new NullLogger<ICommandHandler<FakeCommand>>());
            Assert.IsType<LoggingCommandHandler<FakeCommand>>(sut);

            sut = _innerCommandHandler.WithLogging(new NullLoggerFactory());
            Assert.IsType<LoggingCommandHandler<FakeCommand>>(sut);
        }

        [Fact]
        public void Combining_decorators_wraps_bottom_to_top()
        {
            var sut = _innerCommandHandler
                .WithLogging(new NullLoggerFactory())
                .WithExceptionHandling();

            // The first decorator is the ExceptionHandlingCommandHandler
            Assert.IsType<ExceptionHandlingCommandHandler<FakeCommand>>(sut);

            var fieldInfo = sut.GetType().GetField("_innerCommandHandler", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(fieldInfo);

            var loggingHandlerInstance = fieldInfo.GetValue(sut);
            
            // The second decorator is the LoggingCommandHandler
            Assert.IsType<LoggingCommandHandler<FakeCommand>>(loggingHandlerInstance);

            fieldInfo = loggingHandlerInstance.GetType().GetField("_innerCommandHandler", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(fieldInfo);

            var innerHandler = fieldInfo.GetValue(loggingHandlerInstance);

            // Then we have our actual commandhandler
            Assert.Same(innerHandler, _innerCommandHandler);
        }
    }
}
