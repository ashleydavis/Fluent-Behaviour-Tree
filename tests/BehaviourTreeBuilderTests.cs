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
        public void cant_create_an_unested_action_node()
        {
            Init();

            Assert.Throws<ApplicationException>(() =>
                {
                    testObject
                         .Do("some-node-1", t => BehaviourTreeStatus.Running)
                         .Build();
                }
            );
        }

        [Fact]
        public void can_create_inverter_node()
        {
            Init();

            var node = testObject
                .Inverter("some-inverter")
                    .Do("some-node", t =>BehaviourTreeStatus.Success)
                .End()
                .Build();

            Assert.IsType<InverterNode>(node);
            Assert.Equal(BehaviourTreeStatus.Failure, node.Tick(new TimeData()));
        }

        [Fact]
        public void can_invert_an_inverter()
        {
            Init();

            var node = testObject
                .Inverter("some-inverter")
                    .Inverter("some-other-inverter")
                        .Do("some-node", t => BehaviourTreeStatus.Success)
                    .End()
                .End()
                .Build();

            Assert.IsType<InverterNode>(node);
            Assert.Equal(BehaviourTreeStatus.Success, node.Tick(new TimeData()));
        }

        [Fact]
        public void adding_more_than_a_single_child_to_inverter_throws_exception()
        {
            Init();

            Assert.Throws<ApplicationException>(() =>
            {
                testObject
                    .Inverter("some-inverter")
                        .Do("some-node", t => BehaviourTreeStatus.Success)
                        .Do("some-other-node", t => BehaviourTreeStatus.Success)
                    .End()
                    .Build();
            });
        }

    }
}
