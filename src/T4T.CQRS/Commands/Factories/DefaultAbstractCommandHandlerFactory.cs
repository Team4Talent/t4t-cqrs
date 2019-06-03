using System;
using Autofac;
using Autofac.Features.Indexed;

namespace T4T.CQRS.Commands.Factories
{
    public class DefaultAbstractCommandHandlerFactory : IAbstractCommandHandlerFactory
    {
        private readonly IIndex<Type, ICommandHandlerFactory> _concreteFactories;
        private readonly IComponentContext _componentContext;

        public DefaultAbstractCommandHandlerFactory(IIndex<Type, ICommandHandlerFactory> concreteFactories, IComponentContext componentContext)
        {
            _concreteFactories = concreteFactories;
            _componentContext = componentContext;
        }

        public ICommandHandlerFactory GetFactoryForCommand<T>()
            where T : class
        {
            return _concreteFactories.TryGetValue(typeof(T), out var concreteFactory)
                ? concreteFactory
                : new DefaultCommandHandlerFactory(_componentContext);
        }
    }
}
