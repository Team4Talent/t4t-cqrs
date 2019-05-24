using System.Security.Claims;
using T4T.CQRS.Commands;

namespace T4T.CQRS.Extensions
{
    public static class ICommandHandlerExtensions
    {
        public static ICommandHandler<TCommand> WithExceptionHandling<TCommand>(this ICommandHandler<TCommand> commandHandler)
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
            return new WithRequiredClaimCommandHandler<TCommand>(commandHandler, claim, principal);
        }
    }
}
