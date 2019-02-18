using System;
using System.Security.Claims;
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
            return new WithRequiredClaimQueryHandler<TQuery, TResult>(queryHandler, claim, principal);
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

        public static IQueryHandler<TQuery, TResult> WithExceptionHandling<TQuery, TResult>(this IQueryHandler<TQuery, TResult> queryHandler)
            where TQuery : class
            where TResult : ExecutionResult
        {
            return new ExceptionHandlingQueryHandler<TQuery, TResult>(queryHandler);
        }

        public static IQueryHandler<TQuery, TResult> WithUserId<TQuery, TResult>(
            this IQueryHandler<TQuery, TResult> queryHandler,
            string userId,
            Func<TResult, string> userIdAccessor)
            where TQuery : class
            where TResult : ExecutionResult
        {
            return new WithUserIdQueryHandler<TQuery, TResult>(queryHandler, userId, userIdAccessor);
        }
    }
}
