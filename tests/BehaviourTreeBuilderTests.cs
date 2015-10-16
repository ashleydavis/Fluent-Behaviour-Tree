using fluent_behaviour_tree;
using fluent_behaviour_tree.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace tests
{
    public class BehaviourTreeBuilderTests
    {
        BehaviourTreeBuilder testObject;

        void Init()
        {
            testObject = new BehaviourTreeBuilder();
        }

        [Fact]
        public void can_create_action_node()
        {
            Init();

            var invokeCount = 0;

            var node = BehaviourTreeBuilder
                .Do("some-node", t =>
                {
                    ++invokeCount;
                    return BehaviourTreeStatus.Running;
                });

            Assert.IsType<ActionNode>(node);
            Assert.Equal(BehaviourTreeStatus.Running, node.Tick(new TimeData()));
            Assert.Equal(1, invokeCount);
        }
    }
}
