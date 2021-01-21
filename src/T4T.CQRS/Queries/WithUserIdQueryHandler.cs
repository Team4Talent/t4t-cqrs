using System;
using System.Threading;
using System.Threading.Tasks;
using T4T.CQRS.Execution;

namespace T4T.CQRS.Queries
{
    public class WithUserIdQueryHandler<TUserId, TQuery, TResult> : IQueryHandler<TQuery, TResult>
        where TQuery : class
        where TUserId : IEquatable<TUserId>
        where TResult : ExecutionResult
    {
        private readonly IQueryHandler<TQuery, TResult> _innerQueryHandler;
        private readonly TUserId _userId;
        private readonly Func<TResult, TUserId> _userIdAccessor;

        public WithUserIdQueryHandler(
            IQueryHandler<TQuery, TResult> innerQueryHandler,
            TUserId userId,
            Func<TResult, TUserId> userIdAccessor)
        {
            _innerQueryHandler = innerQueryHandler;
            _userId = userId;
            _userIdAccessor = userIdAccessor;
        }

        public async Task<TResult> Handle(TQuery query,
            CancellationToken cancellationToken = default)
        {
            if (_userId == null)
                return ExecutionResult.Forbidden().As<TResult>();

            var result = await _innerQueryHandler.Handle(query, cancellationToken);
            if (!result.Success)
                return result;

            var userId = _userIdAccessor.Invoke(result);
            if (userId == null || !_userId.Equals(userId))
                return ExecutionResult.Forbidden().As<TResult>();

            return result;
        }
    }
}