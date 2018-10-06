using Moq;

namespace CineworldAlerter.Core.Tests.Mocks
{
    public abstract class MockBase<TInterfaceType>
        where TInterfaceType : class
    {
        private readonly MockBehavior _mockBehavior;

        private Mock<TInterfaceType> _mock;

        public Mock<TInterfaceType> Mock => _mock ?? (_mock = new Mock<TInterfaceType>(_mockBehavior));

        public TInterfaceType Object => Mock.Object;

        protected MockBase(MockBehavior mockBehavior = MockBehavior.Default)
            => _mockBehavior = mockBehavior;
    }
}