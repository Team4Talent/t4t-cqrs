using System.Collections.Generic;
using T4T.CQRS.Execution;
using T4T.CQRS.Samples.API.Model;

namespace T4T.CQRS.Samples.API.Queries.Results
{
    // Queryresults always inherit from ExecutionResult
    // T4T.CQRS.Api uses this ExecutionResult to determine, amongst others, status codes
    public class GetTodosQueryResult : ExecutionResult
    {
        public IEnumerable<TodoItem> Todos { get; set; }

        public GetTodosQueryResult(IEnumerable<TodoItem> todos)
        {
            Todos = todos;
        }
    }
}
