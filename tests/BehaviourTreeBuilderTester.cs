using FluentBehaviourTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace tests
{
    public class BehaviourTreeBuilderTester
    {
        BehaviourTreeBuilder testObject;
        IBehaviourTreeNode btree1;

        void Init()
        {
            testObject = new BehaviourTreeBuilder();
            initBehavior();
        }

        [Fact]
        public void testOneTree()
        {
            Init();

            btree1.Tick(new TimeData(1));

        }
        void initBehavior()
        {
            Console.WriteLine("Initializing Behavior Tree");

            btree1 = testObject
                    .Sequence("Sequence1")
                        .Do("Sequence-1 - Action 1", t =>
                        {
                            return actionSuccess(t, "Sequence 1 - Action 1");
                        })
                        .Do("Sequence1 - Action 2", t =>
                        {
                            return actionSuccess(t, "Sequence 1 - Action 2");
                        })

                    .Selector("Selector-With-Condition")
                        .Condition("Selector1-Condition", t => { return evalActionTrue(t); })
                            .Do("Selector1-Action1", t => { return actionFail(t, "Selector 1 - Action 1"); })
                            .Do("Selector1-Action2", t => { return actionSuccess(t, "Selector 1 - Action 2"); })
                            .Do("Selector1-Action3", t => { return actionSuccess(t, "Selector 1 - Action 3"); })
                        .End()

                    .Parallel("Parallel1", 2, 2).
                        Do("Paralle1-Action1", t =>
                        {
                            return actionSuccess(t, "Parallel1 - Action1");
                        })
                        .Do("Parallel2-Action2", t =>
                        {
                            return actionSuccess(t, "Parallel1-Action2");
                        })
                        .Do("Parallel2-Action3", t =>
                        {
                            return actionSuccess(t, "Parallel1-Action3");
                        })
                    .End()

                    .Sequence("Sequence2")
                        .Do("Sequence2-Action1", t =>
                        {
                            return actionSuccess(t, "Sequence2-Actionl");
                        })
                        .Do("Sequence2-Action2", t =>
                        {
                            return actionSuccess(t, "Sequence2-Action2");
                        })
                        .Do("Sequence2-Action3", t =>
                        {
                            return actionSuccess(t, "Sequence2-Action3");
                        })
                .Build();
            Console.WriteLine("Finished Buidling Behavior Tree !");
        }
        public bool evalActionTrue(TimeData t)
        {
            return true;
        }
        public bool evalActionFalse(TimeData t)
        {
            return false;
        }
        public BehaviourTreeStatus actionSuccess(TimeData t, string aValue)
        {
            //Console.WriteLine(aValue+" at Delta time:"+t.deltaTime);
            Console.WriteLine(aValue + " --> Action Successful ! at Delta time:" + t.deltaTime);
            return BehaviourTreeStatus.Success;
        }
        public BehaviourTreeStatus actionFail(TimeData t, string aValue)
        {
            // Console.WriteLine(aValue+ " at Delta time:"+t.deltaTime);
            Console.WriteLine(aValue + " --> Action Failed ! at Delta time:" + t.deltaTime);
            //throw new ApplicationException("Node Failure to Execute !!");
            return BehaviourTreeStatus.Failure;
        }


    }
}
