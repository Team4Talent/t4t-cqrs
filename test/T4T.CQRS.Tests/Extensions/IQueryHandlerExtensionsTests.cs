using System.Reflection;
using System.Security.Claims;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using T4T.CQRS.Extensions;
using T4T.CQRS.Queries;
using T4T.CQRS.Tests.Fakes;
using T4T.CQRS.Tests.Queries;
using Xunit;

namespace T4T.CQRS.Tests.Extensions
{
    public class IQueryHandlerExtensionsTests
    {
        private readonly IQueryHandler<FakeQuery, FakeQueryResult> _innerQueryHandler =
            new Mock<IQueryHandler<FakeQuery, FakeQueryResult>>().Object;

        [Fact]
        public void WithExceptionHandling_wraps_the_innerQueryHandler_with_ExceptionHandlingQueryHandler()
        {
            var sut = _innerQueryHandler.WithExceptionHandling();
            Assert.IsType<ExceptionHandlingQueryHandler<FakeQuery, FakeQueryResult>>(sut);
        }

        [Fact]
        public void WithAnyOfTheseClaims_wraps_the_innerQueryHandler_with_WithAnyOfTheseClaimsQueryHandler()
        {
            var sut = _innerQueryHandler.WithAnyOfTheseClaims(new ClaimsPrincipal());
            Assert.IsType<WithAnyOfTheseClaimsQueryHandler<FakeQuery, FakeQueryResult>>(sut);
        }

        [Fact]
        public void WithRequiredClaim_wraps_the_innerQueryHandler_with_WithRequiredClaimQueryHandler()
        {
            var sut = _innerQueryHandler.WithRequiredClaim(new Claim("", ""), new ClaimsPrincipal());
            Assert.IsType<WithRequiredClaimQueryHandler<FakeQuery, FakeQueryResult>>(sut);
        }

        [Fact]
        public void WithLogging_wraps_the_innerQueryHandler_with_WithLoggingQueryHandler()
        {
            var sut = _innerQueryHandler.WithLogging(new NullLogger<IQueryHandler<FakeQuery, FakeQueryResult>>());
            Assert.IsType<LoggingQueryHandler<FakeQuery, FakeQueryResult>>(sut);

            sut = _innerQueryHandler.WithLogging(new NullLoggerFactory());
            Assert.IsType<LoggingQueryHandler<FakeQuery, FakeQueryResult>>(sut);
        }

        [Fact]
        public void WithLogging_wraps_the_innerQueryHandler_with_WithUserIdQueryHandler()
        {
            var sut = _innerQueryHandler.WithUserId(0, result => 0);
            Assert.IsType<WithUserIdQueryHandler<int, FakeQuery, FakeQueryResult>>(sut);
        }

        [Fact]
        public void Combining_decorators_wraps_bottom_to_top()
        {
            var sut = _innerQueryHandler
                .WithLogging(new NullLoggerFactory())
                .WithExceptionHandling();

            // The first decorator is the ExceptionHandlingQueryHandler
            Assert.IsType<ExceptionHandlingQueryHandler<FakeQuery, FakeQueryResult>>(sut);

            var fieldInfo = sut.GetType().GetField("_innerQueryHandler", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(fieldInfo);

            var loggingHandlerInstance = fieldInfo.GetValue(sut);
            
            // The second decorator is the LoggingQueryHandler
            Assert.IsType<LoggingQueryHandler<FakeQuery, FakeQueryResult>>(loggingHandlerInstance);

            fieldInfo = loggingHandlerInstance.GetType().GetField("_innerQueryHandler", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(fieldInfo);

            var innerHandler = fieldInfo.GetValue(loggingHandlerInstance);

            // Then we have our actual QueryHandler
            Assert.Same(innerHandler, _innerQueryHandler);
        }
    }
}
