using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using T4T.CQRS.Execution;

namespace T4T.CQRS.Queries
{
    public class WithRequiredClaimQueryHandler<TQuery, TResult> : IQueryHandler<TQuery, TResult>
        where TQuery : class
        where TResult : ExecutionResult
    {
        private readonly IQueryHandler<TQuery, TResult> _innerQueryHandler;
        private readonly Claim _claim;
        private readonly ClaimsPrincipal _principal;

        public WithRequiredClaimQueryHandler(
            IQueryHandler<TQuery, TResult> innerQueryHandler, 
            Claim claim, 
            ClaimsPrincipal principal)
        {
            _innerQueryHandler = innerQueryHandler;
            _claim = claim;
            _principal = principal;
        }

        public async Task<TResult> Handle(TQuery query,
            CancellationToken cancellationToken = default)
        {
            if (_principal.HasClaim(_claim.Type, _claim.Value))
                return await _innerQueryHandler.Handle(query, cancellationToken);

            return ExecutionResult.Forbidden().As<TResult>();
        }
    }
}
