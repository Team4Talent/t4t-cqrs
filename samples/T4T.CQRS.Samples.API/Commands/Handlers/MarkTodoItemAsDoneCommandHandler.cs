using System.Threading;
using System.Threading.Tasks;
using T4T.CQRS.Commands;
using T4T.CQRS.Execution;

namespace T4T.CQRS.Samples.API.Commands.Handlers
{
    public class MarkTodoItemAsDoneCommandHandler : ICommandHandler<MarkTodoItemAsDoneCommand>
    {
        // Inject any dependencies you need here, e.g.:
        // private readonly IRepository<TodoItem> _repository;

        public Task<ExecutionResult> Handle(
            MarkTodoItemAsDoneCommand command, 
            CancellationToken cancellationToken = default)
        {
            // Handle your command here, e.g.:
            // var item = await _repository.GetById(command.Id, cancellationToken);
            // item.IsDone = true;
            // await _repository.Save(item);

            // CommandHandlers return an ExecutionResult to tell consumers how handling the command went.
            // Haters gonna hate.
            var result = ExecutionResult.Succeeded();
            return Task.FromResult(result);

            // ExecutionResult exposes a set of factory methods, e.g.:
            // var result = ExecutionResult.Forbidden("You are not authorized to access this item."); //T4T.CQRS.Api will return this message to the client along with status code 403.
        }
    }
}
