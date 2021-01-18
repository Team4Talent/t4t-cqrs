using System.Threading;
using System.Threading.Tasks;
using T4T.CQRS.Execution;

namespace T4T.CQRS.Queries
{
    public interface IQueryHandler<in TQuery, TResult>
        where TQuery : class
        where TResult : ExecutionResult
    {
        /// <summary>
        ///     Asynchronously handles the given query <typeparamref name="TQuery" />.
        /// </summary>
        /// <param name="query">The query to handle.</param>
        /// <param name="cancellationToken">A token to cancel the asynchronous operation with.</param>
        /// <returns>An instance of <typeparamref name="TResult" />, which derives from <see cref="ExecutionResult" />.</returns>
        Task<TResult> Handle(TQuery query, CancellationToken cancellationToken = default);
    }
}