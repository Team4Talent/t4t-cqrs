namespace T4T.CQRS.Execution
{
    /// <summary>
    /// Represents an error that occurred during execution of a query or a command,
    /// but nothing fatal for the execution.
    /// </summary>
    public class ExecutionWarning
    {
        public string Message { get; }
        public ExecutionWarningType Type { get; }

        public ExecutionWarning(string message, ExecutionWarningType type)
        {
            Message = message;
            Type = type;
        }
    }
}
