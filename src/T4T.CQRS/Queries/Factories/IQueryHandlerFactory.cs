using T4T.CQRS.Execution;

namespace T4T.CQRS.Queries.Factories
{
    public interface IQueryHandlerFactory
    {
        /// <summary>
        ///     Creates an <see cref="IQueryHandler{TQuery,TResult}" /> for the given query <typeparamref name="TQuery" />.
        /// </summary>
        /// <typeparam name="TQuery">The type of the query.</typeparam>
        /// <typeparam name="TResult">The type of the result. Derives from <see cref="ExecutionResult" />.</typeparam>
        /// <returns>An <see cref="IQueryHandler{TQuery,TResult}" /> for type <typeparamref name="TQuery" />.</returns>
        IQueryHandler<TQuery, TResult> CreateQueryHandler<TQuery, TResult>()
            where TQuery : class
            where TResult : ExecutionResult;
    }
}