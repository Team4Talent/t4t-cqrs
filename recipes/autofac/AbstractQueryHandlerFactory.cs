using System;
using Autofac;
using Autofac.Features.Indexed;

public class AbstractQueryHandlerFactory : IAbstractQueryHandlerFactory
{
    private readonly IIndex<Type, IQueryHandlerFactory> _concreteFactories;
    private readonly IComponentContext _componentContext;

    public AbstractQueryHandlerFactory(IIndex<Type, IQueryHandlerFactory> concreteFactories,
        IComponentContext componentContext)
    {
        _concreteFactories = concreteFactories;
        _componentContext = componentContext;
    }

    public IQueryHandlerFactory GetFactoryForQuery<T>()
        where T : class
    {
        // See if a concrete registration exists for type T. If it doesn't, return a default QueryHandlerFactory.
        // This enables switching out factories for given QueryHandlers without touching the consuming controllers/classes/...
        return _concreteFactories.TryGetValue(typeof(T), out var concreteFactory)
            ? concreteFactory
            : new QueryHandlerFactory(_componentContext);
    }
}
