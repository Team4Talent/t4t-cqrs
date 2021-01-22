using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using T4T.CQRS.Execution;
using T4T.CQRS.Queries;
using T4T.CQRS.Tests.Fakes;
using Xunit;

namespace T4T.CQRS.Tests.Queries
{
    public class WithAnyOfTheseClaimsQueryHandlerTests
    {
        private readonly IQueryHandler<FakeQuery, FakeQueryResult> _innerQueryHandler;
        private readonly Claim _principalClaim1 = new Claim("Race", "Tauren");
        private readonly Claim _principalClaim2 = new Claim("Faction", "Horde");
        private readonly ClaimsPrincipal _principal;

        public WithAnyOfTheseClaimsQueryHandlerTests()
        {
            var handlerMock = new Mock<IQueryHandler<FakeQuery, FakeQueryResult>>(MockBehavior.Strict);
            handlerMock.Setup(h => h.Handle(It.IsAny<FakeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ExecutionResult.Succeeded().As<FakeQueryResult>());

            _innerQueryHandler = handlerMock.Object;

            var principalMock = new Mock<ClaimsPrincipal>(MockBehavior.Loose);
            principalMock.Setup(p => p.Claims).Returns(new List<Claim> {_principalClaim1, _principalClaim2});
            principalMock.Setup(p => p.HasClaim(
                    It.Is<string>(c => c == _principalClaim1.Type || c == _principalClaim2.Type),
                    It.Is<string>(c => c == _principalClaim1.Value || c == _principalClaim2.Value)))
                .Returns(true);

            _principal = principalMock.Object;
        }

        [Fact]
        public async Task When_the_principal_has_one_matching_claim_then_the_Handler_succeeds()
        {
            var sut = new WithAnyOfTheseClaimsQueryHandler<FakeQuery, FakeQueryResult>(_innerQueryHandler, _principal, _principalClaim1);
            var result = await sut.Handle(new FakeQuery());

            Assert.True(result.Success);
        }

        [Fact]
        public async Task When_the_principal_has_multiple_matching_claims_then_the_Handler_succeeds()
        {
            var sut = new WithAnyOfTheseClaimsQueryHandler<FakeQuery, FakeQueryResult>(_innerQueryHandler, _principal, _principalClaim1, _principalClaim2);
            var result = await sut.Handle(new FakeQuery());

            Assert.True(result.Success);
        }

        [Fact]
        public async Task When_the_principal_has_no_matching_claims_then_the_Handler_fails()
        {
            var nonMatchingClaim = new Claim("Faction", "Alliance");

            var sut = new WithAnyOfTheseClaimsQueryHandler<FakeQuery, FakeQueryResult>(_innerQueryHandler, _principal, nonMatchingClaim);
            var result = await sut.Handle(new FakeQuery());

            Assert.False(result.Success);
            Assert.NotEmpty(result.Errors);
        }
    }
}
