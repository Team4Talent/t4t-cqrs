using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using T4T.CQRS.Execution;

namespace T4T.CQRS.Queries
{
    public class LoggingQueryHandler<TQuery, TResult> : IQueryHandler<TQuery, TResult>
        where TQuery : class
        where TResult : ExecutionResult
    {
        private readonly IQueryHandler<TQuery, TResult> _innerQueryHandler;
        private readonly ILogger<LoggingQueryHandler<TQuery, TResult>> _logger;

        public LogLevel LogLevel { get; }

        public LoggingQueryHandler(
            IQueryHandler<TQuery, TResult> innerQueryHandler, 
            ILogger<LoggingQueryHandler<TQuery, TResult>> logger, 
            LogLevel logLevel = LogLevel.Warning)
        {
            _innerQueryHandler = innerQueryHandler;
            _logger = logger;

            LogLevel = logLevel;
        }

        public LoggingQueryHandler(
            IQueryHandler<TQuery, TResult> innerQueryHandler,
            ILoggerFactory loggerFactory,
            LogLevel logLevel = LogLevel.Warning)
        {
            _innerQueryHandler = innerQueryHandler;
            _logger = loggerFactory?.CreateLogger<LoggingQueryHandler<TQuery, TResult>>() ??
                      new NullLogger<LoggingQueryHandler<TQuery, TResult>>();

            LogLevel = logLevel;
        }

        public async Task<TResult> Handle(TQuery query, 
            CancellationToken cancellationToken = default)
        {
            var executionResult = await _innerQueryHandler.Handle(query, cancellationToken);

            if (LogLevel >= LogLevel.Error)
                LogErrors(executionResult);

            if (LogLevel >= LogLevel.Information)
                LogWarnings(executionResult);

            if (LogLevel == LogLevel.Trace)
                LogTrace(executionResult);

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
            _logger.LogTrace($"Handled Query {typeof(TQuery).Name}, result: ", executionResult);
        }
    }
}
