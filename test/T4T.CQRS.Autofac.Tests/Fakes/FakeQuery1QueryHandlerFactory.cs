using T4T.CQRS.Execution;
using T4T.CQRS.Queries;
using T4T.CQRS.Queries.Factories;

namespace T4T.CQRS.Autofac.Tests.Fakes
{
    public class FakeQuery1QueryHandlerFactory : IQueryHandlerFactory
    {
        public IQueryHandler<TQuery, TResult> CreateQueryHandler<TQuery, TResult>() 
            where TQuery : class
            where TResult : ExecutionResult
        {
            throw new System.NotImplementedException();
        }
    }
}
