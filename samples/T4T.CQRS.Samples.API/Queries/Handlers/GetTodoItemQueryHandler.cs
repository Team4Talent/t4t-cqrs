using System.Threading;
using System.Threading.Tasks;
using T4T.CQRS.Queries;
using T4T.CQRS.Samples.API.Model;
using T4T.CQRS.Samples.API.Queries.Results;

namespace T4T.CQRS.Samples.API.Queries.Handlers
{
    public class GetTodoItemQueryHandler : IQueryHandler<GetTodoItemQuery, GetTodoItemQueryResult>
    {
        // Inject any dependencies you need here, e.g.:
        // private readonly IRepository<TodoItem> _repository;

        public Task<GetTodoItemQueryResult> Handle(
            GetTodoItemQuery query, 
            CancellationToken cancellationToken = default)
        {
            // Execute your actual query here, e.g.:
            // var item = await _repository.GetById(query.Id, cancellationToken);

            var item = new TodoItem();
            var result = new GetTodoItemQueryResult(item);

            // You also have access to all of ExecutionResult's factory methods, e.g.:
            // var foo = ExecutionResult
            //  .Forbidden("You are not authorized to access this item.") //T4T.CQRS.Api will return this message to the client
            //  .As<GetTodoItemQueryResult>();

            return Task.FromResult(result);
        }
    }
}
