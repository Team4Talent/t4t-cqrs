using System;
using System.Reflection;
using Autofac;
using Autofac.Core.Registration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using T4T.CQRS.Autofac.Tests.Fakes;
using T4T.CQRS.Queries;
using Xunit;

namespace T4T.CQRS.Autofac.Tests
{
    public class QueryHandlerFactoryTests
    {
        private readonly IComponentContext _componentContext;

        public QueryHandlerFactoryTests()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<FakeQuery1Handler>().As<IQueryHandler<FakeQuery1, FakeQueryResult>>();
            containerBuilder.RegisterType<NullLoggerFactory>().As<ILoggerFactory>();

            _componentContext = containerBuilder.Build();
        }

        [Fact]
        public void When_a_QueryHandler_is_registered_then_it_is_decorated_and_returned()
        {
            var sut = new QueryHandlerFactory(_componentContext);
            var result = sut.CreateQueryHandler<FakeQuery1, FakeQueryResult>();

            // The first decorator is the ExceptionHandlingQueryHandler
            Assert.IsType<ExceptionHandlingQueryHandler<FakeQuery1, FakeQueryResult>>(result);

            var fieldInfo = result.GetType().GetField("_innerQueryHandler", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(fieldInfo);

            var loggingHandlerInstance = fieldInfo.GetValue(result);
            
            // The second decorator is the LoggingQueryHandler
            Assert.IsType<LoggingQueryHandler<FakeQuery1, FakeQueryResult>>(loggingHandlerInstance);

            fieldInfo = loggingHandlerInstance.GetType().GetField("_innerQueryHandler", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(fieldInfo);

            var innerHandler = fieldInfo.GetValue(loggingHandlerInstance);

            // Then we have our actual queryhandler
            Assert.IsType<FakeQuery1Handler>(innerHandler);
        }

        [Fact]
        public void When_no_handler_is_registered_then_a_ComponentNotRegisteredException_is_thrown()
        {
            var sut = new QueryHandlerFactory(_componentContext);
            Assert.Throws<ComponentNotRegisteredException>(sut.CreateQueryHandler<FakeQuery2, FakeQueryResult>);
        }
    }
}
