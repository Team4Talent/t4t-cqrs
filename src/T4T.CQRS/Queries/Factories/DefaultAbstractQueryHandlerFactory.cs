using System;
using Autofac;
using Autofac.Features.Indexed;

namespace T4T.CQRS.Queries.Factories
{
    public class DefaultAbstractQueryHandlerFactory : IAbstractQueryHandlerFactory
    {
        private readonly IIndex<Type, IQueryHandlerFactory> _concreteFactories;
        private readonly IComponentContext _componentContext;

        public DefaultAbstractQueryHandlerFactory(IIndex<Type, IQueryHandlerFactory> concreteFactories,
            IComponentContext componentContext)
        {
            _concreteFactories = concreteFactories;
            _componentContext = componentContext;
        }

        public IQueryHandlerFactory GetFactoryForQuery<T>()
            where T : class
        {
            return _concreteFactories.TryGetValue(typeof(T), out var concreteFactory)
                ? concreteFactory
                : new DefaultQueryHandlerFactory(_componentContext);
        }
    }
}
