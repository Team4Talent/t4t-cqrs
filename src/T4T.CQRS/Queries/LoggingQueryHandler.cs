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
        private readonly ILogger<IQueryHandler<TQuery, TResult>> _logger;

        public LogLevel LogLevel { get; }

        public LoggingQueryHandler(
            IQueryHandler<TQuery, TResult> innerQueryHandler, 
            ILogger<IQueryHandler<TQuery, TResult>> logger, 
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
            _logger = loggerFactory?.CreateLogger<IQueryHandler<TQuery, TResult>>() ?? new NullLogger<IQueryHandler<TQuery, TResult>>();

            LogLevel = logLevel;
        }

        public async Task<TResult> Handle(TQuery query, 
            CancellationToken cancellationToken = default)
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
                _logger.LogTrace($"Handled Query {typeof(TQuery).Name}, result: ", result);
            }

            var executionResult = await _innerQueryHandler.Handle(query, cancellationToken);

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
