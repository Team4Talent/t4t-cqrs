using Microsoft.Extensions.Logging;
using T4T.CQRS.Commands;

namespace T4T.CQRS.Extensions
{
    public static class ICommandHandlerExtensions
    {
        public static ICommandHandler<T> WithExceptionHandling<T>(this ICommandHandler<T> commandHandler)
            where T : class
        {
            return new ExceptionHandlingCommandHandler<T>(commandHandler);
        }

        public static ICommandHandler<T> WithLogging<T>(this ICommandHandler<T> commandHandler,
            ILogger<LoggingCommandHandler<T>> logger,
            LogLevel logLevel)
            where T : class
        {
            return new LoggingCommandHandler<T>(commandHandler, logger, logLevel);
        }

        public static ICommandHandler<T> WithLogging<T>(this ICommandHandler<T> commandHandler,
            ILoggerFactory loggerFactory,
            LogLevel logLevel)
            where T : class
        {
            return new LoggingCommandHandler<T>(commandHandler, loggerFactory, logLevel);
        }
    }
}
