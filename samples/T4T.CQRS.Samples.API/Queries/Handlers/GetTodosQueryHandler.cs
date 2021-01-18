using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using T4T.CQRS.Queries;
using T4T.CQRS.Samples.API.Model;
using T4T.CQRS.Samples.API.Queries.Results;

namespace T4T.CQRS.Samples.API.Queries.Handlers
{
    public class GetTodosQueryHandler : IQueryHandler<GetTodosQuery, GetTodosQueryResult>
    {
        // Inject any dependencies you need here, e.g.:
        // private readonly IRepository<TodoItem> _repository;

        public Task<GetTodosQueryResult> Handle(
            GetTodosQuery query, 
            CancellationToken cancellationToken = default)
        {
            // Execute your actual query here, e.g.:
            // var items = await _repository.GetAllItems(cancellationToken);

            var items = Enumerable.Empty<TodoItem>();
            var result = new GetTodosQueryResult(items);

            // You also have access to all of ExecutionResult's factory methods, e.g.:
            // var foo = ExecutionResult
            //  .Forbidden("You are not authorized to access these todos.") //T4T.CQRS.Api will return this message to the client
            //  .As<GetTodosQueryResult>();

            return Task.FromResult(result);
        }
    }
}
