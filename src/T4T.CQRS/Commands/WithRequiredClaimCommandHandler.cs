using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using T4T.CQRS.Execution;

namespace T4T.CQRS.Commands
{
    public class WithRequiredClaimCommandHandler<TCommand> : ICommandHandler<TCommand>
        where TCommand : class
    {
        private readonly ICommandHandler<TCommand> _innerCommandHandler;
        private readonly Claim _claim;
        private readonly ClaimsPrincipal _principal;

        public WithRequiredClaimCommandHandler(
            ICommandHandler<TCommand> innerCommandHandler,
            Claim claim,
            ClaimsPrincipal principal)
        {
            _innerCommandHandler = innerCommandHandler;
            _claim = claim;
            _principal = principal;
        }

        public async Task<ExecutionResult> Handle(TCommand command, 
            CancellationToken cancellationToken = default)
        {
            if (_principal.HasClaim(_claim.Type, _claim.Value))
                return await _innerCommandHandler.Handle(command, cancellationToken);

            return ExecutionResult.Forbidden();
        }
    }
}
