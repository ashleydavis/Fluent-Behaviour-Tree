namespace tests
{
    using FluentBehaviourTree;
    using Xunit;

    public class ActionNodeTests
    {
        [Fact]
        public void can_run_action()
        {
            var time = new TimeData();

            var invokeCount = 0;
            var testObject =
                new ActionNode(
                    "some-action",
                    t =>
                    {
                        Assert.Equal(time, t);

                        ++invokeCount;
                        return BehaviourTreeStatus.Running;
                    }
                );

            Assert.Equal(BehaviourTreeStatus.Running, testObject.Tick(time));
            Assert.Equal(1, invokeCount);
        }
    }
}
