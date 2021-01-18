using System;
using System.Threading;
using System.Threading.Tasks;
using T4T.CQRS.Execution;

namespace T4T.CQRS.Commands
{
    public class ExceptionHandlingCommandHandler<T> : ICommandHandler<T>
        where T : class
    {
        private readonly ICommandHandler<T> _innerCommandHandler;

        public ExceptionHandlingCommandHandler(ICommandHandler<T> innerCommandHandler)
        {
            _innerCommandHandler = innerCommandHandler;
        }

        public async Task<ExecutionResult> Handle(T command,
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await _innerCommandHandler.Handle(command, cancellationToken);
            }
            catch (Exception e)
            {
                return ExecutionResult.FromException(e);
            }
        }
    }
}