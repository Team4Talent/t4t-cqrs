using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using T4T.CQRS.Execution;

namespace T4T.CQRS.Commands
{
    public class LoggingCommandHandler<T> : ICommandHandler<T>
        where T : class
    {
        private readonly ICommandHandler<T> _innerCommandHandler;
        private readonly ILogger<LoggingCommandHandler<T>> _logger;

        public LogLevel LogLevel { get; }

        public LoggingCommandHandler(
            ICommandHandler<T> innerCommandHandler, 
            ILogger<LoggingCommandHandler<T>> logger,
            LogLevel logLevel = LogLevel.Warning)
        {
            _innerCommandHandler = innerCommandHandler;
            _logger = logger;

            LogLevel = logLevel;
        }

        public LoggingCommandHandler(
            ICommandHandler<T> innerCommandHandler,
            ILoggerFactory loggerFactory,
            LogLevel logLevel = LogLevel.Warning)
        {
            _innerCommandHandler = innerCommandHandler;
            _logger = loggerFactory?.CreateLogger<LoggingCommandHandler<T>>() ??
                      new NullLogger<LoggingCommandHandler<T>>();

            LogLevel = logLevel;
        }

        public async Task<ExecutionResult> Handle(T command, CancellationToken cancellationToken = default)
        {
            var executionResult = await _innerCommandHandler.Handle(command, cancellationToken);

            if (LogLevel >= LogLevel.Error)
            {
                LogErrors(executionResult);
            }
            else if (LogLevel >= LogLevel.Information)
            {
                LogErrors(executionResult);
                LogWarnings(executionResult);
            }
            else if (LogLevel == LogLevel.Trace)
            {
                LogErrors(executionResult);
                LogWarnings(executionResult);
                LogTrace(executionResult);
            }  

            return executionResult;
        }

        private void LogErrors(ExecutionResult executionResult)
        {
            foreach (var error in executionResult.Errors)
                _logger.LogError(error.Message, error);
        }

        private void LogWarnings(ExecutionResult executionResult)
        {
            foreach (var warning in executionResult.Warnings)
                _logger.LogWarning(warning, executionResult);
        }

        private void LogTrace(ExecutionResult executionResult)
        {
            _logger.LogTrace($"Handled Command {typeof(T).Name}, result: ", executionResult);
        }
    }
}
