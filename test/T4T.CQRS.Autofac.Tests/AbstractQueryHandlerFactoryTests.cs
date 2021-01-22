using System;
using Autofac;
using Autofac.Features.Indexed;
using Moq;
using T4T.CQRS.Autofac.Tests.Fakes;
using T4T.CQRS.Queries.Factories;
using Xunit;

namespace T4T.CQRS.Autofac.Tests
{
    public class AbstractQueryHandlerFactoryTests
    {
        private readonly IComponentContext _componentContext;
        private readonly IIndex<Type, IQueryHandlerFactory> _concreteFactories;

        public AbstractQueryHandlerFactoryTests()
        {
            IQueryHandlerFactory concreteFactory = new FakeQuery1QueryHandlerFactory();
            IQueryHandlerFactory defaultFactory = new QueryHandlerFactory(_componentContext);

            _componentContext = new Mock<IComponentContext>(MockBehavior.Strict).Object;
            var concreteFactoriesMock = new Mock<IIndex<Type, IQueryHandlerFactory>>(MockBehavior.Strict);
            concreteFactoriesMock.Setup(c =>
                    c.TryGetValue(typeof(FakeQuery1), out concreteFactory))
                .Returns(true);

            concreteFactoriesMock.Setup(c => 
                    c.TryGetValue(typeof(FakeQuery2), out defaultFactory))
                .Returns(false);

            _concreteFactories = concreteFactoriesMock.Object;
        }

        [Fact]
        public void If_a_concrete_Factory_is_registered_then_that_is_returned()
        {
            var sut = new AbstractQueryHandlerFactory(_concreteFactories, _componentContext);
            var result = sut.GetFactoryForQuery<FakeQuery1>();

            Assert.IsType<FakeQuery1QueryHandlerFactory>(result);
        }

        [Fact]
        public void If_no_concrete_Factory_is_registered_then_the_Default_factory_is_returned()
        {
            var sut = new AbstractQueryHandlerFactory(_concreteFactories, _componentContext);
            var result = sut.GetFactoryForQuery<FakeQuery2>();

            Assert.IsType<QueryHandlerFactory>(result);
        }
    }
}
