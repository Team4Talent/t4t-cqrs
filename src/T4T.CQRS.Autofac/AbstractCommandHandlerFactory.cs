using System;
using Autofac;
using Autofac.Features.Indexed;
using T4T.CQRS.Commands.Factories;

namespace T4T.CQRS.Autofac
{
    public class AbstractCommandHandlerFactory : IAbstractCommandHandlerFactory
    {
        private readonly IIndex<Type, ICommandHandlerFactory> _concreteFactories;
        private readonly IComponentContext _componentContext;

        public AbstractCommandHandlerFactory(IIndex<Type, ICommandHandlerFactory> concreteFactories, IComponentContext componentContext)
        {
            _concreteFactories = concreteFactories;
            _componentContext = componentContext;
        }

        public ICommandHandlerFactory GetFactoryForCommand<T>()  where T : class
            => _concreteFactories.TryGetValue(typeof(T), out var concreteFactory)
                ? concreteFactory
                : new CommandHandlerFactory(_componentContext);
    }
}
