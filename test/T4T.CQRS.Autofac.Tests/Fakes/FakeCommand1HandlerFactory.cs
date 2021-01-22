using T4T.CQRS.Commands;
using T4T.CQRS.Commands.Factories;

namespace T4T.CQRS.Autofac.Tests.Fakes
{
    public class FakeCommand1HandlerFactory : ICommandHandlerFactory
    {
        public ICommandHandler<T> CreateCommandHandler<T>() where T : class
        {
            throw new System.NotImplementedException();
        }
    }
}
