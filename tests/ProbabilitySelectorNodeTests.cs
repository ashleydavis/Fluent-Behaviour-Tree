namespace tests
{
    using FluentBehaviourTree;
    using Moq;
    using Xunit;

    public class ProbabilitySelectorNodeTests
    {
        ProbabilitySelectorNode testObject;

        void Init()
        {
            testObject = new ProbabilitySelectorNode("some-selector");
        }

        [Fact]
        public void runs_the_first_node_if_it_succeeds()
        {
            Init();

            var time = new TimeData();

            var mockChild1 = new Mock<IBehaviourTreeNode>();
            mockChild1
                .Setup(m => m.Tick(time))
                .Returns(BehaviourTreeStatus.Success);

            var mockChild2 = new Mock<IBehaviourTreeNode>();
            mockChild2
                .Setup(m => m.Tick(time))
                .Returns(BehaviourTreeStatus.Running);

            var mockChild3 = new Mock<IBehaviourTreeNode>();
            mockChild3
                .Setup(m => m.Tick(time))
                .Returns(BehaviourTreeStatus.Failure);

            testObject.AddChild(mockChild1.Object);
            testObject.AddChild(mockChild2.Object);
            testObject.AddChild(mockChild3.Object);

            for (int i = 1; i < 100; i++)
            {
                Assert.InRange(testObject.Tick(time), BehaviourTreeStatus.Success, BehaviourTreeStatus.Running);
            }
            mockChild1.Verify(m => m.Tick(time), Times.Between(0, 33, Range.Inclusive));
            mockChild2.Verify(m => m.Tick(time), Times.Between(33, 100, Range.Inclusive));
            mockChild3.Verify(m => m.Tick(time), Times.Between(0, 33, Range.Inclusive));
        }
    }
}

