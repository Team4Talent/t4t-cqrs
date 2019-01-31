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

        public static ExecutionError InternalServerError(string message)
            => new ExecutionError(message, ExecutionErrorType.InternalServerError);

        public static ExecutionError BadRequest(string message) 
            => new ExecutionError(message, ExecutionErrorType.BadRequest);

        public static ExecutionError NotFound(string message)
            => new ExecutionError(message, ExecutionErrorType.NotFound);
    }
}
