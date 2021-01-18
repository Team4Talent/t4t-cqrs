using System;
using Autofac;
using Microsoft.Extensions.Logging;
using T4T.CQRS.Commands;
using T4T.CQRS.Commands.Factories;
using T4T.CQRS.Extensions;

namespace T4T.CQRS.Autofac
{
    public class CommandHandlerFactory : ICommandHandlerFactory
    {
        private readonly IComponentContext _container;

        public CommandHandlerFactory(IComponentContext container)
        {
            _container = container;
        }

        public ICommandHandler<T> CreateCommandHandler<T>() 
            where T : class
        {
            var commandHandler = _container.Resolve<ICommandHandler<T>>() ?? 
                                 throw new ArgumentException($"Could not resolve an ICommandHandler for {typeof(T).Name}.", innerException: null);
            var loggerFactory = _container.Resolve<ILoggerFactory>();

            return new ExceptionHandlingCommandHandler<T>(commandHandler)
                .WithLogging(loggerFactory, LogLevel.Warning);
        }
    }
}

