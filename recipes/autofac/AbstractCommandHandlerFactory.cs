using System;
using Autofac;
using Autofac.Features.Indexed;

public class AbstractCommandHandlerFactory : IAbstractCommandHandlerFactory
{
    private readonly IIndex<Type, ICommandHandlerFactory> _concreteFactories;
    private readonly IComponentContext _componentContext;

    public AbstractCommandHandlerFactory(IIndex<Type, ICommandHandlerFactory> concreteFactories, IComponentContext componentContext)
    {
        _concreteFactories = concreteFactories;
        _componentContext = componentContext;
    }

    public ICommandHandlerFactory GetFactoryForCommand<T>()
        where T : class
    {
        // See if a concrete registration exists for type T. If it doesn't, return a default CommandHandlerFactory.
        // This enables switching out factories for given CommandHandlers without touching the consuming controllers/classes/...
        return _concreteFactories.TryGetValue(typeof(T), out var concreteFactory)
            ? concreteFactory
            : new CommandHandlerFactory(_componentContext);
    }
}
