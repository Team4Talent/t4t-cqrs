using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using T4T.CQRS.Execution;

namespace T4T.CQRS.Commands
{
    public class WithAnyOfTheseClaimsCommandHandler<TCommand> : ICommandHandler<TCommand>
        where TCommand : class
    {
        private readonly ICommandHandler<TCommand> _innerCommandHandler;
        private readonly ClaimsPrincipal _principal;
        private readonly Claim[] _claims;

        public WithAnyOfTheseClaimsCommandHandler(
            ICommandHandler<TCommand> innerCommandHandler,
            ClaimsPrincipal principal,
            params Claim[] claims)
        {
            _innerCommandHandler = innerCommandHandler;
            _principal = principal;
            _claims = claims;
        }

        public async Task<ExecutionResult> Handle(TCommand command, 
            CancellationToken cancellationToken = default)
        {
            if (_claims.Any(c => _principal.HasClaim(c.Type, c.Value)))
                return await _innerCommandHandler.Handle(command, cancellationToken);

            return ExecutionResult.Forbidden();
        }
    }
}
