using System.Threading;
using System.Threading.Tasks;
using Moq;
using T4T.CQRS.Api.Tests.Fakes;
using T4T.CQRS.Commands;
using T4T.CQRS.Commands.Factories;
using T4T.CQRS.Execution;
using T4T.CQRS.Queries;
using T4T.CQRS.Queries.Factories;
using Xunit;

namespace T4T.CQRS.Api.Tests
{
    public class ApiControllerBaseTests
    {
        protected IAbstractCommandHandlerFactory AbstractCommandHandlerFactory;
        protected IAbstractQueryHandlerFactory AbstractQueryHandlerFactory;

        public class CommandTests : ApiControllerBaseTests
        {
            private readonly Mock<IAbstractCommandHandlerFactory> _abstractCommandHandlerFactoryMock;
            private readonly Mock<ICommandHandlerFactory> _commandHandlerFactoryMock;

            public CommandTests()
            {
                var commandHandlerMock = new Mock<ICommandHandler<FakeCommand>>();
                commandHandlerMock.Setup(c => c.Handle(It.IsAny<FakeCommand>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(ExecutionResult.Succeeded());

                _commandHandlerFactoryMock = new Mock<ICommandHandlerFactory>(MockBehavior.Strict);
                _commandHandlerFactoryMock.Setup(c => c.CreateCommandHandler<FakeCommand>())
                    .Returns(commandHandlerMock.Object)
                    .Verifiable();

                _abstractCommandHandlerFactoryMock = new Mock<IAbstractCommandHandlerFactory>(MockBehavior.Strict);
                _abstractCommandHandlerFactoryMock.Setup(c => c.GetFactoryForCommand<FakeCommand>())
                    .Returns(_commandHandlerFactoryMock.Object)
                    .Verifiable();

                AbstractCommandHandlerFactory = _abstractCommandHandlerFactoryMock.Object;
            }

            [Fact]
            public async Task HandleCommand_creates_a_Factory_and_handles_the_Command()
            {
                var sut = new TestApiController(AbstractCommandHandlerFactory, AbstractQueryHandlerFactory);
                var result = await sut.HandleCommand(new FakeCommand());

                _abstractCommandHandlerFactoryMock.Verify(c => c.GetFactoryForCommand<FakeCommand>(), Times.Once);
                _commandHandlerFactoryMock.Verify(c => c.CreateCommandHandler<FakeCommand>(), Times.Once);

                Assert.True(result.Success);
            }
        }

        public class QueryTests : ApiControllerBaseTests
        {
            private readonly Mock<IAbstractQueryHandlerFactory> _abstractQueryHandlerFactoryMock;
            private readonly Mock<IQueryHandlerFactory> _queryHandlerFactoryMock;

            public QueryTests()
            {
                var queryHandlerMock = new Mock<IQueryHandler<FakeQuery, FakeQueryResult>>(MockBehavior.Strict);
                queryHandlerMock.Setup(c => c.Handle(It.IsAny<FakeQuery>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(ExecutionResult.Succeeded().As<FakeQueryResult>());

                _queryHandlerFactoryMock = new Mock<IQueryHandlerFactory>(MockBehavior.Strict);
                _queryHandlerFactoryMock.Setup(c => c.CreateQueryHandler<FakeQuery, FakeQueryResult>())
                    .Returns(queryHandlerMock.Object)
                    .Verifiable();

                _abstractQueryHandlerFactoryMock = new Mock<IAbstractQueryHandlerFactory>(MockBehavior.Strict);
                _abstractQueryHandlerFactoryMock.Setup(c => c.GetFactoryForQuery<FakeQuery>())
                    .Returns(_queryHandlerFactoryMock.Object)
                    .Verifiable();

                AbstractQueryHandlerFactory = _abstractQueryHandlerFactoryMock.Object;
            }

            [Fact]
            public async Task HandleQuery_creates_a_Factory_and_handles_the_Query()
            {
                var sut = new TestApiController(AbstractCommandHandlerFactory, AbstractQueryHandlerFactory);
                var result = await sut.HandleQuery<FakeQuery, FakeQueryResult>(new FakeQuery());

                _abstractQueryHandlerFactoryMock.Verify(c => c.GetFactoryForQuery<FakeQuery>(), Times.Once);
                _queryHandlerFactoryMock.Verify(c => c.CreateQueryHandler<FakeQuery, FakeQueryResult>(), Times.Once);

                Assert.True(result.Success);
                Assert.IsType<FakeQueryResult>(result);
            }
        }

        public class TestApiController : ApiControllerBase
        {
            public TestApiController(
                IAbstractCommandHandlerFactory abstractCommandHandlerFactory, 
                IAbstractQueryHandlerFactory abstractQueryHandlerFactory) 
                : base(abstractCommandHandlerFactory, abstractQueryHandlerFactory)
            {
            }
        }
    }
}
