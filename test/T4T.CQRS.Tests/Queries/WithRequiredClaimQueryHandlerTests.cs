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
    public class WithRequiredClaimQueryHandlerTests
    {
        private readonly IQueryHandler<FakeQuery, FakeQueryResult> _innerQueryHandler;
        private readonly Claim _requiredClaim = new Claim("Faction", "Horde");

        public WithRequiredClaimQueryHandlerTests()
        {
            var handlerMock = new Mock<IQueryHandler<FakeQuery, FakeQueryResult>>(MockBehavior.Strict);
            handlerMock.Setup(h => h.Handle(It.IsAny<FakeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ExecutionResult.Succeeded().As<FakeQueryResult>());

            _innerQueryHandler = handlerMock.Object;
        }

        [Fact]
        public async Task When_the_principal_has_the_required_Claim_then_the_Handler_succeeds()
        {
            var principalMock = new Mock<ClaimsPrincipal>(MockBehavior.Loose);
            principalMock.Setup(p => p.HasClaim(_requiredClaim.Type, _requiredClaim.Value)).Returns(true);

            var sut = new WithRequiredClaimQueryHandler<FakeQuery, FakeQueryResult>(_innerQueryHandler, principalMock.Object, _requiredClaim);
            var result = await sut.Handle(new FakeQuery());

            Assert.True(result.Success);
        }

        [Fact]
        public async Task When_the_principal_does_not_have_the_required_Claim_then_the_Handler_fails()
        {
            var principalMock = new Mock<ClaimsPrincipal>(MockBehavior.Loose);
            principalMock.Setup(p => p.HasClaim(_requiredClaim.Type, _requiredClaim.Value)).Returns(false);

            var sut = new WithRequiredClaimQueryHandler<FakeQuery, FakeQueryResult>(_innerQueryHandler, principalMock.Object, _requiredClaim);
            var result = await sut.Handle(new FakeQuery());

            Assert.False(result.Success);
            Assert.NotEmpty(result.Errors);
        }
    }
}
