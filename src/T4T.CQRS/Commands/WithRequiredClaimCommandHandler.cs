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
        private readonly ClaimsPrincipal _principal;
        private readonly Claim _claim;

        public WithRequiredClaimCommandHandler(
            ICommandHandler<TCommand> innerCommandHandler,
            ClaimsPrincipal principal,
            Claim claim)
        {
            _innerCommandHandler = innerCommandHandler;
            _principal = principal;
            _claim = claim;
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
