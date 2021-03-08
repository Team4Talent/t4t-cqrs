using System.Threading;
using System.Threading.Tasks;
using T4T.CQRS.Autofac.Tests.Fakes.Commands;
using T4T.CQRS.Commands;
using T4T.CQRS.Execution;

namespace T4T.CQRS.Autofac.Tests.Fakes
{
    public class FakeCommand1Handler : ICommandHandler<FakeCommand1>
    {
        public Task<ExecutionResult> Handle(FakeCommand1 command, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }
    }
}
