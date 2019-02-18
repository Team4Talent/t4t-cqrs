namespace T4T.CQRS.Queries.Factories
{
    /// <summary>
    /// An abstract factory that will return a concrete factory to create an <see cref="IQueryHandler{T,TResult}"/>.
    /// </summary>
    public interface IAbstractQueryHandlerFactory
    {
        IQueryHandlerFactory GetFactoryForQuery<T>()
            where T : class;
    }
}
