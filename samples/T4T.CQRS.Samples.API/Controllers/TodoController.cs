using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using T4T.CQRS.Api;
using T4T.CQRS.Commands.Factories;
using T4T.CQRS.Queries.Factories;
using T4T.CQRS.Samples.API.Commands;
using T4T.CQRS.Samples.API.Queries;
using T4T.CQRS.Samples.API.Queries.Results;

namespace T4T.CQRS.Samples.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ApiControllerBase
    {
        public TodoController(
            IAbstractCommandHandlerFactory abstractCommandHandlerFactory, 
            IAbstractQueryHandlerFactory abstractQueryHandlerFactory) 
            : base(abstractCommandHandlerFactory, abstractQueryHandlerFactory)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken token = default)
        {
            var query = new GetTodosQuery();

            // HandleQuery is graciously provided by T4T.CQRS.Api.ApiControllerBase
            var result = await HandleQuery<GetTodosQuery, GetTodosQueryResult>(query, token);

            // T4T.CQRS.Api's ToActionResult extension method will transform the ExecutionResult in a nice REST response
            return result.ToActionResult();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, CancellationToken token = default)
        {
            var query = new GetTodoItemQuery(id);
            var result = await HandleQuery<GetTodoItemQuery, GetTodoItemQueryResult>(query, token);

            // If for example "ExecutionResult.Forbidden(message)" was invoked in any of the handlers, ToActionResult will build an appropriate StatusCodeResult
            // and include the message in the response body.
            return result.ToActionResult();
        }

        [HttpPost("{id}/markasdone")]
        public async Task<IActionResult> MarkAsDone(int id, CancellationToken token = default)
        {
            var command = new MarkTodoItemAsDoneCommand(id);
            var result = await HandleCommand(command, token);
            return result.ToActionResult();
        }
    }
}
