using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Contrib.ExpressionBuilders.Logging;
using T4T.CQRS.Execution;
using T4T.CQRS.Queries;
using Xunit;
using Log = Moq.Contrib.ExpressionBuilders.Logging.Log;

namespace T4T.CQRS.Tests.Queries
{
    public class LoggingQueryHandlerTests
    {
        private const string ErrorMessage = "Shit's on fire yo";
        private const string WarningMessage = "This warning is so great, it's unbelievable.";

        private readonly IQueryHandler<FakeQuery, FakeQueryResult> _innerQueryHandler;
        private readonly Mock<ILogger<IQueryHandler<FakeQuery, FakeQueryResult>>> _verifiableLoggerMock;

        public LoggingQueryHandlerTests()
        {
            var executionResult = new ExecutionResult();
            executionResult.AddError(ErrorMessage, ExecutionErrorType.InternalServerError);
            executionResult.AddWarning(WarningMessage);

            var innerHandlerMock = new Mock<IQueryHandler<FakeQuery, FakeQueryResult>>(MockBehavior.Strict);
            innerHandlerMock.Setup(c => c.Handle(It.IsAny<FakeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(executionResult.As<FakeQueryResult>());

            _innerQueryHandler = innerHandlerMock.Object;

            // logger.LogError, logger.LogWarning etc are extension methods and can't be used in verification.
            // We can only verify the call to "ILogger.Log<FormattedLogValues>(LogLevel, EventId, FormattedLogValues, Exception, Func)".
            _verifiableLoggerMock = new Mock<ILogger<IQueryHandler<FakeQuery, FakeQueryResult>>>(MockBehavior.Loose);
        }

        [Theory]
        [InlineData(LogLevel.Critical)]
        [InlineData(LogLevel.Error)]
        public async Task When_LogLevel_is_Error_or_Critical_then_only_errors_are_logged(LogLevel logLevel)
        {
            var sut = new LoggingQueryHandler<FakeQuery, FakeQueryResult>(
                _innerQueryHandler,
                _verifiableLoggerMock.Object,
                logLevel);

            await sut.Handle(new FakeQuery());

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
            var sut = new LoggingQueryHandler<FakeQuery, FakeQueryResult>(
                _innerQueryHandler,
                _verifiableLoggerMock.Object,
                logLevel);

            await sut.Handle(new FakeQuery());

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
            var sut = new LoggingQueryHandler<FakeQuery, FakeQueryResult>(
                _innerQueryHandler,
                _verifiableLoggerMock.Object,
                logLevel);

            await sut.Handle(new FakeQuery());

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
