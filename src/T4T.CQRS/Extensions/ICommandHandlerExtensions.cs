using System.Security.Claims;
using Microsoft.Extensions.Logging;
using T4T.CQRS.Commands;

namespace T4T.CQRS.Extensions
{
    public static class ICommandHandlerExtensions
    {
        public static ICommandHandler<TCommand> WithExceptionHandling<TCommand>(
            this ICommandHandler<TCommand> commandHandler)
            where TCommand : class
        {
            return new ExceptionHandlingCommandHandler<TCommand>(commandHandler);
        }

        public static ICommandHandler<TCommand> WithAnyOfTheseClaims<TCommand>(
            this ICommandHandler<TCommand> commandHandler,
            ClaimsPrincipal principal,
            params Claim[] claims)
            where TCommand : class
        {
            return new WithAnyOfTheseClaimsCommandHandler<TCommand>(commandHandler, principal, claims);
        }

        public static ICommandHandler<TCommand> WithRequiredClaim<TCommand>(
            this ICommandHandler<TCommand> commandHandler,
            Claim claim,
            ClaimsPrincipal principal)
            where TCommand : class
        {
            return new WithRequiredClaimCommandHandler<TCommand>(commandHandler, principal, claim);
        }

        public static ICommandHandler<TCommand> WithLogging<TCommand>(this ICommandHandler<TCommand> commandHandler,
            ILogger<ICommandHandler<TCommand>> logger,
            LogLevel logLevel = LogLevel.Warning)
            where TCommand : class
        {
            return new LoggingCommandHandler<TCommand>(commandHandler, logger, logLevel);
        }

        public static ICommandHandler<TCommand> WithLogging<TCommand>(this ICommandHandler<TCommand> commandHandler,
            ILoggerFactory loggerFactory,
            LogLevel logLevel = LogLevel.Warning)
            where TCommand : class
        {
            return new LoggingCommandHandler<TCommand>(commandHandler, loggerFactory, logLevel);
        }
    }
}