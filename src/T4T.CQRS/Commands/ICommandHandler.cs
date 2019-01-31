using System.Threading;
using System.Threading.Tasks;
using T4T.CQRS.Execution;

namespace T4T.CQRS.Commands
{
    public interface ICommandHandler<in T>
        where T : class
    {
        /// <summary>
        /// Asynchronously handles the given command <typeparamref name="T"/>.
        /// </summary>
        /// <param name="command">The command to handle.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation with.</param>
        /// <returns>An <see cref="ExecutionResult"/>.</returns>
        Task<ExecutionResult> Handle(T command, CancellationToken cancellationToken = default(CancellationToken));
    }
}
