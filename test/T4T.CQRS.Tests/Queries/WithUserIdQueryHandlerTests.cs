using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using T4T.CQRS.Execution;
using T4T.CQRS.Queries;
using T4T.CQRS.Tests.Fakes;
using Xunit;

namespace T4T.CQRS.Tests.Queries
{
    public class WithUserIdQueryHandlerTests
    {
        private readonly IQueryHandler<FakeQuery, FakeQueryResult> _innerQueryHandler;

        public WithUserIdQueryHandlerTests()
        {
            var handlerMock = new Mock<IQueryHandler<FakeQuery, FakeQueryResult>>(MockBehavior.Strict);
            handlerMock.Setup(h => h.Handle(It.IsAny<FakeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ExecutionResult.Succeeded().As<FakeQueryResult>());

            _innerQueryHandler = handlerMock.Object;
        }

        public class IntegerUserIdTests : WithUserIdQueryHandlerTests
        {
            private const int LoggedInUserId = 1;

            [Fact]
            public async Task When_the_User_Ids_match_then_the_Handler_succeeds()
            {
                static int UserIdAccessor(FakeQueryResult result) => LoggedInUserId;

                var sut = new WithUserIdQueryHandler<int, FakeQuery, FakeQueryResult>(_innerQueryHandler, LoggedInUserId, UserIdAccessor);
                var result = await sut.Handle(new FakeQuery());

                Assert.True(result.Success);
                Assert.Empty(result.Errors);
                Assert.Empty(result.Warnings);
            }

            [Fact]
            public async Task When_the_User_Ids_do_not_match_then_the_Handler_contains_errors()
            {
                static int UserIdAccessor(FakeQueryResult result) => 2;

                var sut = new WithUserIdQueryHandler<int, FakeQuery, FakeQueryResult>(_innerQueryHandler, LoggedInUserId, UserIdAccessor);
                var result = await sut.Handle(new FakeQuery());

                Assert.False(result.Success);
                Assert.NotEmpty(result.Errors);
                Assert.Empty(result.Warnings);

                Assert.Equal(ExecutionErrorType.Forbidden, result.Errors[0].Type);
            }
        }

        public class StringUserIdTests : WithUserIdQueryHandlerTests
        {
            private const string LoggedInUserId = "WoW";

            [Fact]
            public async Task When_the_User_Ids_match_then_the_Handler_succeeds()
            {
                static string UserIdAccessor(FakeQueryResult result) => LoggedInUserId;

                var sut = new WithUserIdQueryHandler<string, FakeQuery, FakeQueryResult>(_innerQueryHandler, LoggedInUserId, UserIdAccessor);
                var result = await sut.Handle(new FakeQuery());

                Assert.True(result.Success);
                Assert.Empty(result.Errors);
                Assert.Empty(result.Warnings);
            }

            [Fact]
            public async Task When_the_User_Ids_do_not_match_then_the_Handler_contains_errors()
            {
                static string UserIdAccessor(FakeQueryResult result) => "Foo";

                var sut = new WithUserIdQueryHandler<string, FakeQuery, FakeQueryResult>(_innerQueryHandler, LoggedInUserId, UserIdAccessor);
                var result = await sut.Handle(new FakeQuery());

                Assert.False(result.Success);
                Assert.NotEmpty(result.Errors);
                Assert.Empty(result.Warnings);

                Assert.Equal(ExecutionErrorType.Forbidden, result.Errors[0].Type);
            }

            [Fact]
            public async Task When_the_User_Id_is_null_then_the_Handler_contains_errors()
            {
                static string UserIdAccessor(FakeQueryResult result) => null;

                var sut = new WithUserIdQueryHandler<string, FakeQuery, FakeQueryResult>(_innerQueryHandler, LoggedInUserId, UserIdAccessor);
                var result = await sut.Handle(new FakeQuery());

                Assert.False(result.Success);
                Assert.NotEmpty(result.Errors);
                Assert.Empty(result.Warnings);

                Assert.Equal(ExecutionErrorType.Forbidden, result.Errors[0].Type);
            }
        }

        public class GuidUserIdTests : WithUserIdQueryHandlerTests
        {
            private readonly Guid _loggedInUserId = Guid.NewGuid();

            [Fact]
            public async Task When_the_User_Ids_match_then_the_Handler_succeeds()
            {
                Guid UserIdAccessor(FakeQueryResult r) => _loggedInUserId;

                var sut = new WithUserIdQueryHandler<Guid, FakeQuery, FakeQueryResult>(_innerQueryHandler, _loggedInUserId, UserIdAccessor);
                var result = await sut.Handle(new FakeQuery());

                Assert.True(result.Success);
                Assert.Empty(result.Errors);
                Assert.Empty(result.Warnings);
            }

            [Fact]
            public async Task When_the_User_Ids_do_not_match_then_the_Handler_contains_errors()
            {
                static Guid UserIdAccessor(FakeQueryResult r) => Guid.NewGuid();

                var sut = new WithUserIdQueryHandler<Guid, FakeQuery, FakeQueryResult>(_innerQueryHandler, _loggedInUserId, UserIdAccessor);
                var result = await sut.Handle(new FakeQuery());

                Assert.False(result.Success);
                Assert.NotEmpty(result.Errors);
                Assert.Empty(result.Warnings);

                Assert.Equal(ExecutionErrorType.Forbidden, result.Errors[0].Type);
            }
        }
    }
}
