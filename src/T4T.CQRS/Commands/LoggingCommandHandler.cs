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
        private readonly ILogger<ICommandHandler<T>> _logger;

        public LoggingCommandHandler(
            ICommandHandler<T> innerCommandHandler,
            ILogger<ICommandHandler<T>> logger,
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
            _logger = loggerFactory?.CreateLogger<ICommandHandler<T>>() ?? new NullLogger<ICommandHandler<T>>();

            LogLevel = logLevel;
        }

        public LogLevel LogLevel { get; }

        public async Task<ExecutionResult> Handle(T command, CancellationToken cancellationToken = default)
        {
            void LogErrors(ExecutionResult result)
            {
                foreach (var error in result.Errors)
                    _logger.LogError(error.Message, error);
            }

            void LogWarnings(ExecutionResult result)
            {
                foreach (var warning in result.Warnings)
                    _logger.LogWarning(warning, result);
            }

            void LogTrace(ExecutionResult result)
            {
                _logger.LogTrace($"Handled Command {typeof(T).Name}, result: ", result);
            }

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
    }
}