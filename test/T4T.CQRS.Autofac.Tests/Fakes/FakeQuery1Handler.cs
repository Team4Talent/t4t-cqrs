using System.Threading;
using System.Threading.Tasks;
using T4T.CQRS.Queries;

namespace T4T.CQRS.Autofac.Tests.Fakes
{
    public class FakeQuery1Handler : IQueryHandler<FakeQuery1, FakeQueryResult>
    {
        public Task<FakeQueryResult> Handle(FakeQuery1 query, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }
    }
}
