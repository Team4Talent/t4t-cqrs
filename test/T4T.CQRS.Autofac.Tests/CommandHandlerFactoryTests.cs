using System.Reflection;
using Autofac;
using Autofac.Core.Registration;
using T4T.CQRS.Autofac.Tests.Fakes;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using T4T.CQRS.Commands;
using Xunit;

namespace T4T.CQRS.Autofac.Tests
{
    public class CommandHandlerFactoryTests
    {
        private readonly IComponentContext _componentContext;

        public CommandHandlerFactoryTests()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<FakeCommand1Handler>().As<ICommandHandler<FakeCommand1>>();
            containerBuilder.RegisterType<NullLoggerFactory>().As<ILoggerFactory>();

            _componentContext = containerBuilder.Build();
        }

        [Fact]
        public void When_a_CommandHandler_is_registered_then_it_is_decorated_and_returned()
        {
            var sut = new CommandHandlerFactory(_componentContext);
            var result = sut.CreateCommandHandler<FakeCommand1>();

            // The first decorator is the ExceptionHandlingCommandHandler
            Assert.IsType<ExceptionHandlingCommandHandler<FakeCommand1>>(result);

            var fieldInfo = result.GetType().GetField("_innerCommandHandler", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(fieldInfo);

            var loggingHandlerInstance = fieldInfo.GetValue(result);
            
            // The second decorator is the LoggingCommandHandler
            Assert.IsType<LoggingCommandHandler<FakeCommand1>>(loggingHandlerInstance);

            fieldInfo = loggingHandlerInstance.GetType().GetField("_innerCommandHandler", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(fieldInfo);

            var innerHandler = fieldInfo.GetValue(loggingHandlerInstance);

            // Then we have our actual commandhandler
            Assert.IsType<FakeCommand1Handler>(innerHandler);
        }

        [Fact]
        public void When_no_handler_is_registered_then_a_ComponentNotRegisteredException_is_thrown()
        {
            var sut = new CommandHandlerFactory(_componentContext);
            Assert.Throws<ComponentNotRegisteredException>(sut.CreateCommandHandler<FakeCommand2>);
        }
    }
}
