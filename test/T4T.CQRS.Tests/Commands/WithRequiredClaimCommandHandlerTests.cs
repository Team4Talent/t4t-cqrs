using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using T4T.CQRS.Commands;
using T4T.CQRS.Execution;
using T4T.CQRS.Tests.Fakes;
using Xunit;

namespace T4T.CQRS.Tests.Commands
{
    public class WithRequiredClaimCommandHandlerTests
    {
        private readonly ICommandHandler<FakeCommand> _innerCommandHandler;
        private readonly Claim _requiredClaim = new Claim("Faction", "Horde");

        public WithRequiredClaimCommandHandlerTests()
        {
            var handlerMock = new Mock<ICommandHandler<FakeCommand>>(MockBehavior.Strict);
            handlerMock.Setup(h => h.Handle(It.IsAny<FakeCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ExecutionResult.Succeeded());

            _innerCommandHandler = handlerMock.Object;
        }

        [Fact]
        public async Task When_the_principal_has_the_required_Claim_then_the_Handler_succeeds()
        {
            var principalMock = new Mock<ClaimsPrincipal>(MockBehavior.Loose);
            principalMock.Setup(p => p.HasClaim(_requiredClaim.Type, _requiredClaim.Value)).Returns(true);

            var sut = new WithRequiredClaimCommandHandler<FakeCommand>(_innerCommandHandler, principalMock.Object, _requiredClaim);
            var result = await sut.Handle(new FakeCommand());

            Assert.True(result.Success);
        }

        [Fact]
        public async Task When_the_principal_does_not_have_the_required_Claim_then_the_Handler_fails()
        {
            var principalMock = new Mock<ClaimsPrincipal>(MockBehavior.Loose);
            principalMock.Setup(p => p.HasClaim(_requiredClaim.Type, _requiredClaim.Value)).Returns(false);

            var sut = new WithRequiredClaimCommandHandler<FakeCommand>(_innerCommandHandler, principalMock.Object, _requiredClaim);
            var result = await sut.Handle(new FakeCommand());

            Assert.False(result.Success);
            Assert.NotEmpty(result.Errors);
        }
    }
}
