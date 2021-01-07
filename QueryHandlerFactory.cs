using System;
using Autofac;
using Microsoft.Extensions.Logging;
using T4T.CQRS.Execution;
using T4T.CQRS.Extensions;

public class QueryHandlerFactory : IQueryHandlerFactory
{
    private readonly IComponentContext _container;

    public QueryHandlerFactory(IComponentContext container)
    {
        _container = container;
    }

    // An example of a "default" QueryHandlerFactory that simply searches Autofac for a specific handler
    // registered for type T and wraps it with some logging and exception handling.
    public IQueryHandler<TQuery, TResult> CreateQueryHandler<TQuery, TResult>() 
        where TQuery : class 
        where TResult : ExecutionResult
    {
        var queryHandler = _container.Resolve<IQueryHandler<TQuery, TResult>>() ??
                           throw new ArgumentException($"Could not resolve an IQueryHandler for {typeof(TQuery).Name}.", innerException: null);
        var loggerFactory = _container.Resolve<ILoggerFactory>();

        return new ExceptionHandlingQueryHandler<TQuery, TResult>(queryHandler)
            .WithLogging(loggerFactory, LogLevel.Warning);
    }
}