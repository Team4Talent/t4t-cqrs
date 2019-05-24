using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using T4T.CQRS.Execution;

namespace T4T.CQRS.Queries
{
    public class WithAnyOfTheseClaimsQueryHandler<TQuery, TResult> : IQueryHandler<TQuery, TResult>
        where TQuery : class
        where TResult : ExecutionResult
    {
        private readonly IQueryHandler<TQuery, TResult> _innerQueryHandler;
        private readonly ClaimsPrincipal _principal;
        private readonly Claim[] _claims;

        public WithAnyOfTheseClaimsQueryHandler(
            IQueryHandler<TQuery, TResult> innerQueryHandler, 
            ClaimsPrincipal principal, 
            params Claim[] claims)
        {
            _innerQueryHandler = innerQueryHandler;
            _principal = principal;
            _claims = claims;
        }

        public async Task<TResult> Handle(TQuery query,
            CancellationToken cancellationToken = default)
        {
            if (_claims.Any(c => _principal.HasClaim(c.Type, c.Value)))
                return await _innerQueryHandler.Handle(query, cancellationToken);

            return ExecutionResult.Forbidden().As<TResult>();
        }
    }
}
