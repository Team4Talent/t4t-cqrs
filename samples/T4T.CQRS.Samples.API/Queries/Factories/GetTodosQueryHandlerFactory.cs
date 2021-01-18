using T4T.CQRS.Execution;
using T4T.CQRS.Queries;
using T4T.CQRS.Queries.Factories;
using T4T.CQRS.Samples.API.Queries.Handlers;

namespace T4T.CQRS.Samples.API.Queries.Factories
{
    public class GetTodosQueryHandlerFactory : IQueryHandlerFactory
    {
        public IQueryHandler<TQuery, TResult> CreateQueryHandler<TQuery, TResult>() 
            where TQuery : class 
            where TResult : ExecutionResult
        {
            return (IQueryHandler<TQuery, TResult>) new GetTodosQueryHandler();
        }
    }
}
