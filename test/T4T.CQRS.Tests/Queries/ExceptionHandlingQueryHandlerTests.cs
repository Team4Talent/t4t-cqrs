using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using T4T.CQRS.Execution;
using T4T.CQRS.Queries;
using T4T.CQRS.Tests.Fakes;
using Xunit;

namespace T4T.CQRS.Tests.Queries
{
    public class ExceptionHandlingQueryHandlerTests
    {
        [Fact]
        public async Task When_no_exceptions_are_thrown_then_a_QueryResult_is_returned()
        {
            var innerHandlerMock = new Mock<IQueryHandler<FakeQuery, FakeQueryResult>>(MockBehavior.Strict);
            innerHandlerMock.Setup(h => h.Handle(It.IsAny<FakeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ExecutionResult.Succeeded().As<FakeQueryResult>());

            var sut = new ExceptionHandlingQueryHandler<FakeQuery, FakeQueryResult>(innerHandlerMock.Object);
            var actualResult = await sut.Handle(new FakeQuery());

            Assert.True(actualResult.Success);
            Assert.IsType<FakeQueryResult>(actualResult);
            Assert.Empty(actualResult.Errors);
            Assert.Empty(actualResult.Warnings);
        }

        [Fact]
        public async Task When_an_exceptions_is_thrown_then_a_QueryResult_is_returned_with_Errors()
        {
            var exception = new InvalidOperationException("You shall not pass!");

            var innerHandlerMock = new Mock<IQueryHandler<FakeQuery, FakeQueryResult>>(MockBehavior.Strict);
            innerHandlerMock.Setup(h => h.Handle(It.IsAny<FakeQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(exception);

            var sut = new ExceptionHandlingQueryHandler<FakeQuery, FakeQueryResult>(innerHandlerMock.Object);
            var actualResult = await sut.Handle(new FakeQuery());

            Assert.False(actualResult.Success);
            Assert.IsType<FakeQueryResult>(actualResult);
            Assert.NotEmpty(actualResult.Errors);
        }
    }
}
