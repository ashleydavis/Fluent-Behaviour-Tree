using FluentBehaviourTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace tests
{
    public class ActionNodeTests
    {
        [Fact]
        public void can_run_action()
        {
            var time = new TimeData();

            var invokeCount = 0;
            IEnumerable<BehaviourTreeStatus> statusValues = Enum.GetValues(typeof(BehaviourTreeStatus)).Cast<BehaviourTreeStatus>();
            statusValues.GetEnumerator();
            var testObject = 
                new ActionNode(
                    "some-action", 
                    t =>
                    {
                        Assert.Equal(time, t);
                        ++invokeCount;
                        return TreeStatus.getStatus(BehaviourTreeStatus.Running);
                    }
                );

            var e = testObject.Tick(time);
            e.MoveNext();
            Assert.Equal(BehaviourTreeStatus.Running,e.Current);
            Assert.Equal(1, invokeCount);            
        }
    }
}
