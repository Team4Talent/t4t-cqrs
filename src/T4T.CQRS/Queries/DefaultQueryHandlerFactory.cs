using System;
using Autofac;
using T4T.CQRS.Execution;

namespace T4T.CQRS.Queries
{
    public class DefaultQueryHandlerFactory : IQueryHandlerFactory
    {
        private readonly IComponentContext _container;

        public DefaultQueryHandlerFactory(IComponentContext container)
        {
            _container = container;
        }

        public IQueryHandler<TQuery, TResult> CreateQueryHandler<TQuery, TResult>() 
            where TQuery : class 
            where TResult : ExecutionResult
        {
            var queryHandler = _container.Resolve<IQueryHandler<TQuery, TResult>>() ??
                               throw new ArgumentException($"Could not resolve an IQueryHandler for {typeof(TQuery).Name}.", innerException: null);

            return new ExceptionHandlingQueryHandler<TQuery, TResult>(queryHandler);
        }
    }
}
