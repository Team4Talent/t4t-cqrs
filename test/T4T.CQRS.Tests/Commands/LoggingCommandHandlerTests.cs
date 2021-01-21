using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Contrib.ExpressionBuilders.Logging;
using T4T.CQRS.Commands;
using T4T.CQRS.Execution;
using Xunit;

namespace T4T.CQRS.Tests.Commands
{
    public class LoggingCommandHandlerTests
    {
        private const string ErrorMessage = "Shit's on fire yo";
        private const string WarningMessage = "This warning is so great, it's unbelievable.";

        private readonly ICommandHandler<FakeCommand> _innerCommandHandler;
        private readonly Mock<ILogger<ICommandHandler<FakeCommand>>> _verifiableLoggerMock;

        public LoggingCommandHandlerTests()
        {
            var executionResult = new ExecutionResult();
            executionResult.AddError(ErrorMessage, ExecutionErrorType.InternalServerError);
            executionResult.AddWarning(WarningMessage);

            var innerCommandHandlerMock = new Mock<ICommandHandler<FakeCommand>>(MockBehavior.Strict);
            innerCommandHandlerMock.Setup(c => c.Handle(It.IsAny<FakeCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(executionResult);

            _innerCommandHandler = innerCommandHandlerMock.Object;

            // logger.LogError, logger.LogWarning etc are extension methods and can't be used in verification.
            // We can only verify the call to "ILogger.Log<FormattedLogValues>(LogLevel, EventId, FormattedLogValues, Exception, Func)".
            _verifiableLoggerMock = new Mock<ILogger<ICommandHandler<FakeCommand>>>(MockBehavior.Loose);
        }

        [Theory]
        [InlineData(LogLevel.Critical)]
        [InlineData(LogLevel.Error)]
        public async Task When_LogLevel_is_Error_or_Critical_then_only_errors_are_logged(LogLevel logLevel)
        {
            var sut = new LoggingCommandHandler<FakeCommand>(
                _innerCommandHandler,
                _verifiableLoggerMock.Object,
                logLevel);

            await sut.Handle(new FakeCommand());

            _verifiableLoggerMock.Verify(
                Log.With.LogLevel(LogLevel.Error).And.LogMessage(ErrorMessage),
                Times.AtLeastOnce);

            _verifiableLoggerMock.Verify(Log.With.LogLevel(LogLevel.Warning), Times.Never);
            _verifiableLoggerMock.Verify(Log.With.LogLevel(LogLevel.Trace), Times.Never);
        }

        [Theory]
        [InlineData(LogLevel.Warning)]
        [InlineData(LogLevel.Information)]
        public async Task When_LogLevel_is_Information_or_Warning_then_errors_and_warnings_are_logged(LogLevel logLevel)
        {
            var sut = new LoggingCommandHandler<FakeCommand>(
                _innerCommandHandler,
                _verifiableLoggerMock.Object,
                logLevel);

            await sut.Handle(new FakeCommand());

            _verifiableLoggerMock.Verify(
                Log.With.LogLevel(LogLevel.Error).And.LogMessage(ErrorMessage),
                Times.AtLeastOnce);

            _verifiableLoggerMock.Verify(
                Log.With.LogLevel(LogLevel.Warning).And.LogMessage(WarningMessage),
                Times.AtLeastOnce);

            _verifiableLoggerMock.Verify(Log.With.LogLevel(LogLevel.Trace), Times.Never);
        }

        [Theory]
        [InlineData(LogLevel.Debug)]
        [InlineData(LogLevel.Trace)]
        public async Task When_LogLevel_is_Debug_or_Trace_then_everything_is_logged(LogLevel logLevel)
        {
            var sut = new LoggingCommandHandler<FakeCommand>(
                _innerCommandHandler,
                _verifiableLoggerMock.Object,
                logLevel);

            await sut.Handle(new FakeCommand());

            _verifiableLoggerMock.Verify(
                Log.With.LogLevel(LogLevel.Error).And.LogMessage(ErrorMessage),
                Times.AtLeastOnce);

            _verifiableLoggerMock.Verify(
                Log.With.LogLevel(LogLevel.Warning).And.LogMessage(WarningMessage),
                Times.AtLeastOnce);

            _verifiableLoggerMock.Verify(
                Log.With.LogLevel(LogLevel.Trace),
                Times.AtLeastOnce);
        }
    }
}
