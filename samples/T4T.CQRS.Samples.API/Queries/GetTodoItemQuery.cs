namespace T4T.CQRS.Samples.API.Queries
{
    // Queries are simple POCOs
    public class GetTodoItemQuery
    {
        public int TodoItemId { get; }

        public GetTodoItemQuery(int todoItemId)
        {
            TodoItemId = todoItemId;
        }
    }
}
