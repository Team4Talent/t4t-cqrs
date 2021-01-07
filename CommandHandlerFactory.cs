using System;
using Autofac;
using Microsoft.Extensions.Logging;
using T4T.CQRS.Extensions;

public class CommandHandlerFactory : ICommandHandlerFactory
{
    private readonly IComponentContext _container;

    public CommandHandlerFactory(IComponentContext container)
    {
        _container = container;
    }

    // An example of a "default" CommandHandlerFactory that simply searches Autofac for a specific handler
    // registered for type T and wraps it with some logging and exception handling.
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

