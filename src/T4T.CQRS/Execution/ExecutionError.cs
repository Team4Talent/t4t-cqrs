namespace T4T.CQRS.Execution
{
    /// <summary>
    /// Represents an error which caused the execution of a query or a command to stop.
    /// </summary>
    public class ExecutionError
    {
        public string Message { get; }
        public ExecutionErrorType Type { get; }

        public ExecutionError(string message, ExecutionErrorType type)
        {
            Message = message;
            Type = type;
        }
    }
}
