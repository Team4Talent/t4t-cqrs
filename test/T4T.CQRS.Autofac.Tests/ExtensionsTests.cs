using System.Reflection;
using Autofac;
using T4T.CQRS.Autofac.Tests.Fakes;
using T4T.CQRS.Autofac.Tests.Fakes.Commands;
using T4T.CQRS.Autofac.Tests.Fakes.Queries;
using T4T.CQRS.Commands.Factories;
using T4T.CQRS.Queries.Factories;
using Xunit;

namespace T4T.CQRS.Autofac.Tests
{
    public class ExtensionsTests
    {
        private readonly IComponentContext _componentContext;

        public ExtensionsTests()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<CQRSModule>();

            containerBuilder.RegisterDefaultFactoryForCommandsInAssembly<FakeCommand1HandlerFactory>(
                Assembly.GetExecutingAssembly(), "T4T.CQRS.Autofac.Tests.Fakes.Commands");

            containerBuilder.RegisterDefaultFactoryForQueriesInAssembly<FakeQuery1QueryHandlerFactory>(
                Assembly.GetExecutingAssembly(), "T4T.CQRS.Autofac.Tests.Fakes.Queries");

            _componentContext = containerBuilder.Build();
        }

        [Fact]
        public void When_an_ICommandHandlerFactory_is_registered_It_is_returned_for_all_types_in_the_assembly()
        {
            var fakeCommand1Factory = _componentContext.ResolveKeyed<ICommandHandlerFactory>(typeof(FakeCommand1));
            Assert.IsType<FakeCommand1HandlerFactory>(fakeCommand1Factory);

            var fakeCommand2Factory = _componentContext.ResolveKeyed<ICommandHandlerFactory>(typeof(FakeCommand2));
            Assert.IsType<FakeCommand1HandlerFactory>(fakeCommand2Factory);
        }

        [Fact]
        public void When_an_IQueryHandlerFactory_is_registered_It_is_returned_for_all_types_in_the_assembly()
        {
            var fakeQuery1Factory = _componentContext.ResolveKeyed<IQueryHandlerFactory>(typeof(FakeQuery1));
            Assert.IsType<FakeQuery1QueryHandlerFactory>(fakeQuery1Factory);

            var fakeQuery2Factory = _componentContext.ResolveKeyed<IQueryHandlerFactory>(typeof(FakeQuery2));
            Assert.IsType<FakeQuery1QueryHandlerFactory>(fakeQuery2Factory);
        }
    }
}
