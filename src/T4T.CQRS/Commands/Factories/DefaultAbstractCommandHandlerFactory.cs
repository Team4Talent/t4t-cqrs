using Autofac;
using Autofac.Features.Indexed;

namespace T4T.CQRS.Commands.Factories
{
    public class DefaultAbstractCommandHandlerFactory : IAbstractCommandHandlerFactory
    {
        private readonly IIndex<string, ICommandHandlerFactory> _concreteFactories;
        private readonly IComponentContext _componentContext;

        public DefaultAbstractCommandHandlerFactory(IIndex<string, ICommandHandlerFactory> concreteFactories, IComponentContext componentContext)
        {
            _concreteFactories = concreteFactories;
            _componentContext = componentContext;
        }

        public ICommandHandlerFactory GetFactoryForCommand<T>()
            where T : class
        {
            return _concreteFactories.TryGetValue(typeof(T).Name, out var concreteFactory)
                ? concreteFactory
                : new DefaultCommandHandlerFactory(_componentContext);
        }
    }
}
