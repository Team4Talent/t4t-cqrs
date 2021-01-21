using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Moq;
using T4T.CQRS.Commands;
using T4T.CQRS.Execution;

namespace T4T.CQRS.Tests.Commands
{
    public class ExceptionHandlingCommandHandlerTests
    {
        [Fact]
        public async Task When_no_exceptions_are_thrown_then_an_ExecutionResult_with_success_is_returned()
        {
            var innerHandlerMock = new Mock<ICommandHandler<FakeCommand>>(MockBehavior.Strict);
            innerHandlerMock.Setup(h => h.Handle(It.IsAny<FakeCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ExecutionResult.Succeeded());

            var sut = new ExceptionHandlingCommandHandler<FakeCommand>(innerHandlerMock.Object);
            var actualResult = await sut.Handle(new FakeCommand());

            Assert.True(actualResult.Success);
            Assert.Empty(actualResult.Errors);
            Assert.Empty(actualResult.Warnings);
        }

        [Fact]
        public async Task When_an_exception_is_thrown_then_an_ExecutionResult_with_errors_is_returned()
        {
            var exception = new InvalidOperationException("You shall not pass!");

            var innerHandlerMock = new Mock<ICommandHandler<FakeCommand>>(MockBehavior.Strict);
            innerHandlerMock.Setup(h => h.Handle(It.IsAny<FakeCommand>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(exception);

            var sut = new ExceptionHandlingCommandHandler<FakeCommand>(innerHandlerMock.Object);
            var actualResult = await sut.Handle(new FakeCommand());

            Assert.False(actualResult.Success);
            Assert.NotEmpty(actualResult.Errors);
        }
    }
}
