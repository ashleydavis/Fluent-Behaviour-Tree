using FluentBehaviourTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using System.Diagnostics;
using System.Reflection;

namespace tests
{
    public class TreeBuilderTest3
    {
        BehaviourTreeBuilder treeBuilder2;
        IBehaviourTreeNode btree1;
        IBehaviourTreeNode btree2;
        Dictionary<string, string> callData;

        string seq1Action1 = "Sequence1Action1";
        string seq1Action2 = "Sequence1Action2";

        string sel1Condition = "Selector1Condition";
        string sel1Action1 = "Selector1Action1";
        string sel1Action2 = "Selector1Action2";
        string sel1Action3 = "Selector1Action4";
        string sel1Action4 = "Selector1Action5";
        string sel1Action5 = "Selector1Action6";
        string sel1Action6 = "Selector1Action7";
        string sel1Action7 = "Selector1Action8";
        string sel1Action8 = "Selector1Action9";
        string sel1Action9 = "Selector1Action10";
        string sel1Action10 = "Selector1Action11";
        string sel1Action11 = "Selector1Action12";
        string sel1Action12 = "Selector1Action13";

        string sel2Condition = "Selector2Condition";
        string sel2Action1 = "Selector2Action1";
        string sel2Action2 = "Selector2Action2";
        string sel2Action3 = "Selector2Action3";
        string sel2Action4 = "Selector2Action4";
        string sel2Action5 = "Selector2Action5";
        string sel2Action6 = "Selector2Action6";
        string sel2Action7 = "Selector2Action7";
        string sel2Action8 = "Selector2Action8";
        string sel2Action9 = "Selector2Action9";
        string sel2Action10 = "Selector2Action10";
        string sel2Action11 = "Selector2Action11";
        string sel2Action12 = "Selector2Action12";

        string FevalActionTrue = "evalActionTrue";
        string FevalActionFalse = "evalActionFalse";
        string FactionSuccess = "actionSuccess";
        string FactionFail = "actionFail";

        void Init()
        {
            treeBuilder2 = new BehaviourTreeBuilder();
            callData = new Dictionary<string, string>();
        }

        [Fact]
        public void testRandomSelector()
        {
            float deltaTime = 523.4f;
            Init();
            initTree1();
            btree1.Tick(new TimeData(deltaTime));
            // Check callData to ensure leaf node was invoked by the Tick.

            // Check Sequence 1 Actions
            Assert.Equal(FactionSuccess, callData[seq1Action1 + deltaTime]);
            Assert.Equal(FactionSuccess, callData[seq1Action2 + deltaTime]);
            // Check Condition Actions
            Assert.Equal(FevalActionTrue, callData[sel1Condition + deltaTime]);

            // the actions will be random and there is a 1/12 chance that it could be the 1st one
            // and may cause the unit test to fail. This line should be commented out for automated
            // unit testing.
            Assert.False(callData.ContainsKey(sel1Action1 + deltaTime));

            // check that exactly only one action was triggered ...
            int numTriggered = 0;
            if (callData.ContainsKey(sel1Action1 + deltaTime))  ++numTriggered;
            if (callData.ContainsKey(sel1Action2 + deltaTime))  ++numTriggered;
            if (callData.ContainsKey(sel1Action3 + deltaTime))  ++numTriggered;
            if (callData.ContainsKey(sel1Action4 + deltaTime))  ++numTriggered;
            if (callData.ContainsKey(sel1Action5 + deltaTime))  ++numTriggered;
            if (callData.ContainsKey(sel1Action6 + deltaTime))  ++numTriggered;
            if (callData.ContainsKey(sel1Action7 + deltaTime))  ++numTriggered;
            if (callData.ContainsKey(sel1Action8 + deltaTime))  ++numTriggered;
            if (callData.ContainsKey(sel1Action9 + deltaTime))  ++numTriggered;
            if (callData.ContainsKey(sel1Action10 + deltaTime)) ++numTriggered;
            if (callData.ContainsKey(sel1Action11 + deltaTime)) ++numTriggered;
            if (callData.ContainsKey(sel1Action12 + deltaTime)) ++numTriggered;
            Assert.Equal(numTriggered, 1);
            // Check 2nd Condition Actions
            Assert.Equal(FevalActionTrue, callData[sel2Condition + deltaTime]);
            Assert.Equal(FactionFail, callData[sel2Action1 + deltaTime]);
            Assert.Equal(FactionFail, callData[sel2Action2 + deltaTime]);
            Assert.Equal(FactionFail, callData[sel2Action3 + deltaTime]);
            Assert.Equal(FactionFail, callData[sel2Action4 + deltaTime]);
            // The 5th Selector will be called because it is the first one that does not fail
            Assert.Equal(FactionSuccess, callData[sel2Action5 + deltaTime]);
            Assert.False(callData.ContainsKey(sel2Action6 + deltaTime));
            Assert.False(callData.ContainsKey(sel2Action7 + deltaTime));
            Assert.False(callData.ContainsKey(sel2Action8 + deltaTime));
            Assert.False(callData.ContainsKey(sel2Action9 + deltaTime));
            Assert.False(callData.ContainsKey(sel2Action10 + deltaTime));
            Assert.False(callData.ContainsKey(sel2Action11 + deltaTime));
            Assert.False(callData.ContainsKey(sel2Action12 + deltaTime));
        }
      
        void initTree1()
        {
            Console.WriteLine("Initializing Behavior Tree 1");

            btree1 = treeBuilder2
                    .Sequence("Sequence1")
                        .Do(seq1Action1, t =>
                        {
                            return actionSuccess(t, seq1Action1);
                        })
                        .Do(seq1Action2, t =>
                        {
                            return actionSuccess(t, seq1Action2);
                        })
                    .End()
                    .RandomSelector("Random-Selector1-With-Condition")
                        .Condition(sel1Condition, t => { return evalActionTrue(t, sel1Condition); })
                            .Do(sel1Action1, t => { return actionSuccess(t,    sel1Action1); })
                            .Do(sel1Action2, t => { return actionSuccess(t,    sel1Action2); })
                            .Do(sel1Action3, t => { return actionSuccess(t,    sel1Action3); })
                            .Do(sel1Action4, t => { return actionSuccess(t,    sel1Action4); })
                            .Do(sel1Action5, t => { return actionSuccess(t, sel1Action5); })
                            .Do(sel1Action6, t => { return actionSuccess(t, sel1Action6); })
                            .Do(sel1Action7, t => { return actionSuccess(t, sel1Action7); })
                            .Do(sel1Action8, t => { return actionSuccess(t, sel1Action8); })
                            .Do(sel1Action9, t => { return actionSuccess(t, sel1Action9); })
                            .Do(sel1Action10, t => { return actionSuccess(t, sel1Action10); })
                            .Do(sel1Action11, t => { return actionSuccess(t, sel1Action11); })
                            .Do(sel1Action12, t => { return actionSuccess(t, sel1Action12); })
                    .End()
                    
                    .Selector("Normal-Selector2-With-Condition")
                        .Condition(sel2Condition, t => { return evalActionTrue(t, sel2Condition); })
                            .Do(sel2Action1, t => { return actionFail(t, sel2Action1); })
                            .Do(sel2Action2, t => { return actionFail(t, sel2Action2); })
                            .Do(sel2Action3, t => { return actionFail(t, sel2Action3); })
                            .Do(sel2Action4, t => { return actionFail(t, sel2Action4); })
                            .Do(sel2Action5, t => { return actionSuccess(t, sel2Action5); })
                            .Do(sel2Action6, t => { return actionSuccess(t, sel2Action6); })
                            .Do(sel2Action7, t => { return actionSuccess(t, sel2Action7); })
                            .Do(sel2Action8, t => { return actionSuccess(t, sel2Action8); })
                            .Do(sel2Action9, t => { return actionSuccess(t, sel2Action9); })
                            .Do(sel2Action10, t => { return actionSuccess(t, sel2Action10); })
                            .Do(sel2Action11, t => { return actionSuccess(t, sel2Action11); })
                            .Do(sel2Action12, t => { return actionSuccess(t, sel2Action12); })
                    .End()

                .Build();
            Console.WriteLine("Finished Buidling Behavior Tree 1 !");
        }
       
        public bool evalActionTrue(TimeData t, string aValue)
        {

            callData.Add(aValue + t.deltaTime, FevalActionTrue);

            return true;
        }
        public bool evalActionFalse(TimeData t, string aValue)
        {    
            callData.Add(aValue + t.deltaTime, FevalActionFalse);
            return false;
        }
        public BehaviourTreeStatus actionSuccess(TimeData t, string aValue)
        {
            
            Console.WriteLine(aValue + " --> Action Successful ! at Delta time:" + t.deltaTime);
            callData.Add(aValue + t.deltaTime, FactionSuccess);
            return BehaviourTreeStatus.Success;
        }
        public BehaviourTreeStatus actionFail(TimeData t, string aValue)
        {
            
            Console.WriteLine(aValue + " --> Action Failed ! at Delta time:" + t.deltaTime);
            //throw new ApplicationException("Node Failure to Execute !!");
            callData.Add(aValue + t.deltaTime, FactionFail);
            return BehaviourTreeStatus.Failure;
        }


    }
}
