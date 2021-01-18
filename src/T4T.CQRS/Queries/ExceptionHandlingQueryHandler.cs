using System;
using System.Threading;
using System.Threading.Tasks;
using T4T.CQRS.Execution;

namespace T4T.CQRS.Queries
{
    public class ExceptionHandlingQueryHandler<TQuery, TResult> : IQueryHandler<TQuery, TResult>
        where TQuery : class
        where TResult : ExecutionResult
    {
        private readonly IQueryHandler<TQuery, TResult> _innerQueryHandler;

        public ExceptionHandlingQueryHandler(IQueryHandler<TQuery, TResult> innerQueryHandler)
        {
            _innerQueryHandler = innerQueryHandler;
        }

        public async Task<TResult> Handle(TQuery query,
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await _innerQueryHandler.Handle(query, cancellationToken);
            }
            catch (Exception e)
            {
                return ExecutionResult.FromException(e).As<TResult>();
            }
        }
    }
}