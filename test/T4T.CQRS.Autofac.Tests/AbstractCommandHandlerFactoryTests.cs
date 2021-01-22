using System;
using Autofac;
using Autofac.Features.Indexed;
using Moq;
using T4T.CQRS.Autofac.Tests.Fakes;
using T4T.CQRS.Commands.Factories;
using Xunit;

namespace T4T.CQRS.Autofac.Tests
{
    public class AbstractCommandHandlerFactoryTests
    {
        private readonly IComponentContext _componentContext;
        private readonly IIndex<Type, ICommandHandlerFactory> _concreteFactories;

        public AbstractCommandHandlerFactoryTests()
        {
            ICommandHandlerFactory concreteFactory = new FakeCommand1HandlerFactory();
            ICommandHandlerFactory defaultFactory = new CommandHandlerFactory(_componentContext);

            _componentContext = new Mock<IComponentContext>(MockBehavior.Strict).Object;
            var concreteFactoriesMock = new Mock<IIndex<Type, ICommandHandlerFactory>>(MockBehavior.Strict);
            concreteFactoriesMock.Setup(c =>
                    c.TryGetValue(typeof(FakeCommand1), out concreteFactory))
                .Returns(true);

            concreteFactoriesMock.Setup(c => 
                    c.TryGetValue(typeof(FakeCommand2), out defaultFactory))
                .Returns(false);

            _concreteFactories = concreteFactoriesMock.Object;
        }

        [Fact]
        public void If_a_concrete_Factory_is_registered_then_that_is_returned()
        {
            var sut = new AbstractCommandHandlerFactory(_concreteFactories, _componentContext);
            var result = sut.GetFactoryForCommand<FakeCommand1>();

            Assert.IsType<FakeCommand1HandlerFactory>(result);
        }

        [Fact]
        public void If_no_concrete_Factory_is_registered_then_the_Default_factory_is_returned()
        {
            var sut = new AbstractCommandHandlerFactory(_concreteFactories, _componentContext);
            var result = sut.GetFactoryForCommand<FakeCommand2>();

            Assert.IsType<CommandHandlerFactory>(result);
        }
    }
}
