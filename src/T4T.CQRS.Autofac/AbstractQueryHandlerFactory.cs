using System;
using Autofac;
using Autofac.Features.Indexed;
using T4T.CQRS.Queries.Factories;

namespace T4T.CQRS.Autofac
{
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

        public IQueryHandlerFactory GetFactoryForQuery<T>() where T : class
            => _concreteFactories.TryGetValue(typeof(T), out var concreteFactory)
                ? concreteFactory
                : new QueryHandlerFactory(_componentContext);
    }
}
