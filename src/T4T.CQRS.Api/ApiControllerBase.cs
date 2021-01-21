using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using T4T.CQRS.Commands.Factories;
using T4T.CQRS.Execution;
using T4T.CQRS.Queries.Factories;

[assembly:InternalsVisibleTo("T4T.CQRS.Api.Tests")]

namespace T4T.CQRS.Api
{
    public abstract class ApiControllerBase : ControllerBase
    {
        private readonly IAbstractCommandHandlerFactory _abstractCommandHandlerFactory;
        private readonly IAbstractQueryHandlerFactory _abstractQueryHandlerFactory;

        protected ApiControllerBase(IAbstractCommandHandlerFactory abstractCommandHandlerFactory,
            IAbstractQueryHandlerFactory abstractQueryHandlerFactory)
        {
            _abstractCommandHandlerFactory = abstractCommandHandlerFactory;
            _abstractQueryHandlerFactory = abstractQueryHandlerFactory;
        }

        protected internal async Task<ExecutionResult> HandleCommand<T>(T command,
            CancellationToken cancellationToken = default)
            where T : class
        {
            var factory = _abstractCommandHandlerFactory.GetFactoryForCommand<T>();
            var commandHandler = factory.CreateCommandHandler<T>();
            return await commandHandler.Handle(command, cancellationToken);
        }

        protected internal async Task<TResult> HandleQuery<TQuery, TResult>(TQuery query,
            CancellationToken cancellationToken = default)
            where TQuery : class
            where TResult : ExecutionResult
        {
            var factory = _abstractQueryHandlerFactory.GetFactoryForQuery<TQuery>();
            var queryHandler = factory.CreateQueryHandler<TQuery, TResult>();
            return await queryHandler.Handle(query, cancellationToken);
        }
    }
}