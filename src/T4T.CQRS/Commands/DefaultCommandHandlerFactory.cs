using System;
using Autofac;

namespace T4T.CQRS.Commands
{
    public class DefaultCommandHandlerFactory : ICommandHandlerFactory
    {
        private readonly IComponentContext _container;

        public DefaultCommandHandlerFactory(IComponentContext container)
        {
            _container = container;
        }

        public ICommandHandler<T> CreateCommandHandler<T>() 
            where T : class
        {
            var commandHandler = _container.Resolve<ICommandHandler<T>>() ?? 
                                 throw new ArgumentException($"Could not resolve an ICommandHandler for {typeof(T).Name}.", innerException: null);
                
            return new ExceptionHandlingCommandHandler<T>(commandHandler);
        }
    }
}
