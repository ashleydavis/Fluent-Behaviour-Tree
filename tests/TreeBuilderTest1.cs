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
    public class TreeBuilderTest1
    {
        BehaviourTreeBuilder treeBuilder1;
        IBehaviourTreeNode btree1;
        IBehaviourTreeNode btree2;
        Dictionary<string, string> callData;
        
        string seq1Action1 = "Sequence1Action1";
        string seq1Action2 = "Sequence1Action2";

        string sel1Condition = "Selector1Condition";
        string sel1Action1 = "Selector1Action1";
        string sel1Action2 = "Selector1Action2";
        string sel1Action3 = "Selector1Action3";

        string par1Action1 = "Paralle1Action1";
        string par1Action2 = "Paralle1Action2";
        string par1Action3 = "Paralle1Action3";

        string seq2Action1 = "Sequence2Action1";
        string seq2Action2 = "Sequence2Action2";
        string seq2Action3 = "Sequence2Action3";

        string FevalActionTrue   = "evalActionTrue";
        string FevalActionFalse  = "evalActionFalse";
        string FactionSuccess    = "actionSuccess";
        string FactionFail       = "actionFail";

        void Init()
        {
            treeBuilder1    = new BehaviourTreeBuilder();
            callData      = new Dictionary<string, string>(); 
        }

        [Fact]
        public void testTreeWithSequenceSelectorSequenceWithNoEndBetweenParents()
        {
            float deltaTime = 89.6f;
            Init();
            initTree1();
            var e= btree1.Tick(new TimeData(deltaTime));
            e.MoveNext();
            // Check callData to ensure leaf node was invoked by the Tick.

            // Check Sequence 1 Actions
            Assert.Equal(FactionSuccess,  callData[seq1Action1+deltaTime]);
            Assert.Equal(FactionSuccess,  callData[seq1Action2 + deltaTime]);

            // Check Condition Actions
            Assert.Equal(FevalActionTrue, callData[sel1Condition + deltaTime]);
            Assert.Equal(FactionFail, callData[sel1Action1 + deltaTime]);
            Assert.Equal(FactionSuccess, callData[sel1Action2 + deltaTime]);
            // Last Selector action is not called because previous one was successful
            Assert.False(callData.ContainsKey(sel1Action3 + deltaTime));
            // Parallel Calls will not be executed b/c it is in selector scope
            Assert.False(callData.ContainsKey(par1Action1 + deltaTime));
            Assert.False(callData.ContainsKey(par1Action2 + deltaTime));
            Assert.False(callData.ContainsKey(par1Action3 + deltaTime));

            // Sequence2 will also be ignored because it is also in selector scope
            Assert.False(callData.ContainsKey(seq2Action1 + deltaTime));
            Assert.False(callData.ContainsKey(seq2Action2 + deltaTime));
            Assert.False(callData.ContainsKey(seq2Action3 + deltaTime));

        }
        [Fact]
        public void testTreeWithSequenceSelectorSequenceWithEndAfterEachParent()
        {
            float deltaTime = 389.87f;
            Init();
            initTree2();
            var e = btree2.Tick(new TimeData(deltaTime));
            e.MoveNext();
            // Check callData to ensure leaf node was invoked by the Tick.

            // Check Sequence 1 Actions
            Assert.Equal(FactionSuccess, callData[seq1Action1 + deltaTime]);
            Assert.Equal(FactionSuccess, callData[seq1Action2 + deltaTime]);

            // Check Condition Actions
            Assert.Equal(FevalActionTrue, callData[sel1Condition + deltaTime]);
            Assert.Equal(FactionFail, callData[sel1Action1 + deltaTime]);
            Assert.Equal(FactionSuccess, callData[sel1Action2 + deltaTime]);
            // Last Selector action is not called because previous one was successful
            Assert.False(callData.ContainsKey(sel1Action3 + deltaTime));
            // Parallel Calls will now be executed b/c it is outside the scope of the selector
            Assert.Equal(FactionSuccess, callData[par1Action1 + deltaTime]);
            Assert.Equal(FactionSuccess, callData[par1Action2 + deltaTime]);
            Assert.Equal(FactionSuccess,callData[par1Action3 + deltaTime]);

            // Sequence2 will also be ignored because of the selector
            Assert.Equal(FactionSuccess,callData[seq2Action1 + deltaTime]);
            Assert.Equal(FactionSuccess,callData[seq2Action2 + deltaTime]);
            Assert.Equal(FactionSuccess,callData[seq2Action3 + deltaTime]);
        }
        void initTree1()
        {
            Console.WriteLine("Initializing Behavior Tree 1");

            btree1 = treeBuilder1
                    .Sequence("Sequence1")
                        .Do(seq1Action1, t =>
                        {
                            return actionSuccess(t, seq1Action1);
                        })
                        .Do(seq1Action2, t =>
                        {
                            return actionSuccess(t, seq1Action2);
                        })

                    .Selector("Selector-With-Condition")
                        .Condition(sel1Condition, t => { return evalActionTrue(t, sel1Condition); })
                            .Do(sel1Action1, t => { return actionFail(t,    sel1Action1); })
                            .Do(sel1Action2, t => { return actionSuccess(t, sel1Action2); })
                            .Do(sel1Action3, t => { return actionSuccess(t, sel1Action3); })
                       

                    .Parallel("Parallel1", 2, 2).
                        Do(par1Action1, t =>
                        {
                            return actionSuccess(t, par1Action1);
                        })
                        .Do(par1Action2, t =>
                        {
                            return actionSuccess(t, par1Action2);
                        })
                        .Do(par1Action3, t =>
                        {
                            return actionSuccess(t, par1Action3);
                        })
                    
                    .Sequence("Sequence2")
                        .Do(seq2Action1, t =>
                        {
                            return actionSuccess(t, seq2Action1);
                        })
                        .Do(seq2Action2, t =>
                        {
                            return actionSuccess(t, seq2Action2);
                        })
                        .Do(seq2Action3, t =>
                        {
                            return actionSuccess(t, seq2Action3);
                        })
                .End()
                .Build();
            Console.WriteLine("Finished Buidling Behavior Tree 1 !");
        }
        void initTree2()
        {
            Console.WriteLine("Initializing Behavior Tree 2");

            btree2 = treeBuilder1
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
                    .Selector("Selector-With-Condition")
                        .Condition(sel1Condition, t => { return evalActionTrue(t, sel1Condition); })
                            .Do(sel1Action1, t => { return actionFail(t, sel1Action1); })
                            .Do(sel1Action2, t => { return actionSuccess(t, sel1Action2); })
                            .Do(sel1Action3, t => { return actionSuccess(t, sel1Action3); })
                    .End()
                    .Parallel("Parallel1", 2, 2).
                        Do(par1Action1, t =>
                        {
                            return actionSuccess(t, par1Action1);
                        })
                        .Do(par1Action2, t =>
                        {
                            return actionSuccess(t, par1Action2);
                        })
                        .Do(par1Action3, t =>
                        {
                            return actionSuccess(t, par1Action3);
                        })
                    
                    .End()
                    .Sequence("Sequence2")
                        .Do(seq2Action1, t =>
                        {
                            return actionSuccess(t, seq2Action1);
                        })
                        .Do(seq2Action2, t =>
                        {
                            return actionSuccess(t, seq2Action2);
                        })
                        .Do(seq2Action3, t =>
                        {
                            return actionSuccess(t, seq2Action3);
                        })
                .End()
                .Build();
            Console.WriteLine("Finished Buidling Behavior Tree 2 !");
        }
        public bool evalActionTrue(TimeData t, string aValue)
        {
             
            StackFrame frame = new StackFrame(2);
            string methodName = frame.GetMethod().Name; //Gets the current method name
            MethodBase method = frame.GetMethod();
            string className = method.DeclaringType.Name; //Gets the current class name
            string caller = className + "." + methodName;
            string thismethod = this.GetType().ToString() + "." + MethodBase.GetCurrentMethod().Name;
            Console.WriteLine("in " + thismethod + " : Caller: " + caller);
            callData.Add( aValue + t.deltaTime, FevalActionTrue);

            return true;
        }
        public bool evalActionFalse(TimeData t, string aValue)
        {
            StackFrame frame = new StackFrame(2);
            string methodName = frame.GetMethod().Name; //Gets the current method name
            MethodBase method = frame.GetMethod();
            string className = method.DeclaringType.Name; //Gets the current class name
            string caller = className + "." + methodName;
            string thismethod = this.GetType().ToString() + "." + MethodBase.GetCurrentMethod().Name;
            Console.WriteLine("in " + thismethod + " : Caller: " + caller);
            callData.Add(aValue + t.deltaTime, FevalActionFalse);
            return false;
        }
        public IEnumerator<BehaviourTreeStatus> actionSuccess(TimeData t, string aValue)
        {
            StackFrame frame = new StackFrame(2);
            string methodName = frame.GetMethod().Name; //Gets the current method name
            MethodBase method = frame.GetMethod();
            string className = method.DeclaringType.Name; //Gets the current class name
            string caller = className + "." + methodName;
            string thismethod = this.GetType().ToString() + "." + MethodBase.GetCurrentMethod().Name;
            Console.WriteLine("in " + thismethod + " : Caller: " + caller);
            Console.WriteLine(aValue + " --> Action Successful ! at Delta time:" + t.deltaTime);
            callData.Add( aValue + t.deltaTime, FactionSuccess);
            yield return BehaviourTreeStatus.Success;
        }
        public IEnumerator<BehaviourTreeStatus> actionFail(TimeData t, string aValue)
        {
            StackFrame frame = new StackFrame(2);
            string methodName = frame.GetMethod().Name; //Gets the current method name
            MethodBase method = frame.GetMethod();
            string className = method.DeclaringType.Name; //Gets the current class name
            string caller = className + "." + methodName;
            string thismethod = this.GetType().ToString() + "." + MethodBase.GetCurrentMethod().Name;
            Console.WriteLine("in " + thismethod + " : Caller: " + caller);
            Console.WriteLine(aValue + " --> Action Failed ! at Delta time:" + t.deltaTime);
            //throw new ApplicationException("Node Failure to Execute !!");
            callData.Add(aValue + t.deltaTime, FactionFail);
            yield  return BehaviourTreeStatus.Failure;
        }


    }
}
