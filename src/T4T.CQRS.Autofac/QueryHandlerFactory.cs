using System;
using Autofac;
using Microsoft.Extensions.Logging;
using T4T.CQRS.Execution;
using T4T.CQRS.Extensions;
using T4T.CQRS.Queries;
using T4T.CQRS.Queries.Factories;

namespace T4T.CQRS.Autofac
{
    public class QueryHandlerFactory : IQueryHandlerFactory
    {
        private readonly IComponentContext _container;

        public QueryHandlerFactory(IComponentContext container)
        {
            _container = container;
        }

        public IQueryHandler<TQuery, TResult> CreateQueryHandler<TQuery, TResult>() 
            where TQuery : class 
            where TResult : ExecutionResult
        {
            var queryHandler = _container.Resolve<IQueryHandler<TQuery, TResult>>();
            var loggerFactory = _container.Resolve<ILoggerFactory>();

            return new LoggingQueryHandler<TQuery, TResult> (queryHandler, loggerFactory)
                .WithExceptionHandling();
        }
    }
}