using System;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using T4T.CQRS.Execution;
using T4T.CQRS.Queries;

namespace T4T.CQRS.Extensions
{
    public static class IQueryHandlerExtensions
    {
        public static IQueryHandler<TQuery, TResult> WithRequiredClaim<TQuery, TResult>(
            this IQueryHandler<TQuery, TResult> queryHandler,
            Claim claim,
            ClaimsPrincipal principal)
            where TQuery : class
            where TResult : ExecutionResult
        {
            return new WithRequiredClaimQueryHandler<TQuery, TResult>(queryHandler, principal, claim);
        }

        public static IQueryHandler<TQuery, TResult> WithAnyOfTheseClaims<TQuery, TResult>(
            this IQueryHandler<TQuery, TResult> queryHandler,
            ClaimsPrincipal principal,
            params Claim[] claims)
            where TQuery : class
            where TResult : ExecutionResult
        {
            return new WithAnyOfTheseClaimsQueryHandler<TQuery, TResult>(queryHandler, principal, claims);
        }

        public static IQueryHandler<TQuery, TResult> WithExceptionHandling<TQuery, TResult>(
            this IQueryHandler<TQuery, TResult> queryHandler)
            where TQuery : class
            where TResult : ExecutionResult
        {
            return new ExceptionHandlingQueryHandler<TQuery, TResult>(queryHandler);
        }

        public static IQueryHandler<TQuery, TResult> WithUserId<TUserId, TQuery, TResult>(
            this IQueryHandler<TQuery, TResult> queryHandler,
            TUserId userId,
            Func<TResult, TUserId> userIdAccessor)
            where TQuery : class
            where TUserId : IEquatable<TUserId>
            where TResult : ExecutionResult
        {
            return new WithUserIdQueryHandler<TUserId, TQuery, TResult>(queryHandler, userId, userIdAccessor);
        }

        public static IQueryHandler<TQuery, TResult> WithLogging<TQuery, TResult>(
            this IQueryHandler<TQuery, TResult> queryHandler,
            ILogger<IQueryHandler<TQuery, TResult>> logger,
            LogLevel logLevel = LogLevel.Warning)
            where TQuery : class
            where TResult : ExecutionResult
        {
            return new LoggingQueryHandler<TQuery, TResult>(queryHandler, logger, logLevel);
        }

        public static IQueryHandler<TQuery, TResult> WithLogging<TQuery, TResult>(
            this IQueryHandler<TQuery, TResult> queryHandler,
            ILoggerFactory loggerFactory,
            LogLevel logLevel = LogLevel.Warning)
            where TQuery : class
            where TResult : ExecutionResult
        {
            return new LoggingQueryHandler<TQuery, TResult>(queryHandler, loggerFactory, logLevel);
        }
    }
}