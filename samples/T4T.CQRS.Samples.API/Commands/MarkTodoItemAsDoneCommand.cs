namespace T4T.CQRS.Samples.API.Commands
{
    // Commands are simple POCOs
    public class MarkTodoItemAsDoneCommand
    {
        public int Id { get; }

        public MarkTodoItemAsDoneCommand(int id)
        {
            Id = id;
        }
    }
}
