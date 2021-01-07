using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using T4T.CQRS.Execution;

namespace T4T.CQRS.Api
{
    public static class ExecutionResultExtensions
    {
        public static IActionResult ToActionResult<TResult>(this TResult queryResult)
            where TResult : ExecutionResult
        {
            // Check errors by order of severity.
            if (queryResult.Errors.Any(e => e.Type == ExecutionErrorType.InternalServerError))
                return new ObjectResult(queryResult) {StatusCode = (int) HttpStatusCode.InternalServerError};

            if (queryResult.Errors.Any(e => e.Type == ExecutionErrorType.BadRequest))
                return new ObjectResult(queryResult) {StatusCode = (int) HttpStatusCode.BadRequest};

            if (queryResult.Errors.Any(e => e.Type == ExecutionErrorType.NotFound))
                return new NotFoundObjectResult(queryResult) {StatusCode = (int) HttpStatusCode.NotFound};

            if (queryResult.Errors.Any(e => e.Type == ExecutionErrorType.Forbidden))
                return new ObjectResult(queryResult) {StatusCode = (int) HttpStatusCode.Forbidden};

            if (queryResult.Errors.Any(e => e.Type == ExecutionErrorType.Unauthorized))
                return new ObjectResult(queryResult) { StatusCode = (int)HttpStatusCode.Unauthorized };

            return new OkObjectResult(queryResult);
        }

        public static IActionResult ToCreatedResult(this ExecutionResult commandResult, string location, string id)
        {
            var result = commandResult.ToActionResult();

            return !(result is OkObjectResult) ? result : new CreatedResult(location,
                    new {id, errors = commandResult.Errors, warnings = commandResult.Warnings, success = commandResult.Success});
        }
    }
}
