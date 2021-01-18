using System;
using System.Threading;
using System.Threading.Tasks;
using T4T.CQRS.Execution;

namespace T4T.CQRS.Queries
{
    public class WithUserIdQueryHandler<TQuery, TResult> : IQueryHandler<TQuery, TResult>
        where TQuery : class
        where TResult : ExecutionResult
    {
        private readonly IQueryHandler<TQuery, TResult> _innerQueryHandler;
        private readonly string _userId;
        private readonly Func<TResult, string> _userIdAccessor;

        public WithUserIdQueryHandler(
            IQueryHandler<TQuery, TResult> innerQueryHandler,
            string userId,
            Func<TResult, string> userIdAccessor)
        {
            _innerQueryHandler = innerQueryHandler;
            _userId = userId;
            _userIdAccessor = userIdAccessor;
        }

        public async Task<TResult> Handle(TQuery query,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(_userId))
                return ExecutionResult.Forbidden().As<TResult>();

            var result = await _innerQueryHandler.Handle(query, cancellationToken);
            if (!result.Success)
                return result;

            var userId = _userIdAccessor.Invoke(result);
            if (string.IsNullOrEmpty(userId) || !_userId.Equals(userId, StringComparison.OrdinalIgnoreCase))
                return ExecutionResult.Forbidden().As<TResult>();

            return result;
        }
    }
}