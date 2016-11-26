using FluentBehaviourTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

namespace tests
{
    /// <summary>
    /// This class unit tests the IENumerator interface of the BehaviourTree to return
    /// each iteration of the node as an enumerated value.  This is useful so that nodes
    /// can spread their execution over several frames to make the UI more responsive.
    /// Nodes that do not need to make use of this feature can just yield success or failure 
    /// immediately after doing their work.
    /// </summary>
    public class treeBuilderTestCoroutine
    {
        BehaviourTreeBuilder treeBuilder;
        IBehaviourTreeNode btree1;
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

        string FevalActionTrue = "evalActionTrue";
        string FevalActionFalse = "evalActionFalse";
        string FactionSuccess = "actionSuccess";
        string FactionFail = "actionFail";

        void Init()
        {
            treeBuilder = new BehaviourTreeBuilder();
            callData = new Dictionary<string, string>();
        }

        [Fact]
        public void testCoroutine() { 
            Log("Before StartCoroutine()");
            Init();
            initTree1();
            Console.WriteLine(" ===> Beginning Btree1 Hierarchy From testCoroutine --> ");
            Console.WriteLine(btree1.getTreeAsString(" --> "));
            IEnumerator<BehaviourTreeStatus> f1 = (coroutine("Some Input Value", (myReturnValue, location) =>
            {
                Log("-- Callback Returned Value from " + location + " is: " + myReturnValue);
            }));
            for (var x = f1; x.MoveNext();)
            {
                Console.WriteLine(" --> Result in testCoroutine is: " + x.Current);
            }
            Log("End Iteration of coroutine()");
            Console.WriteLine(" ===> Ending Btree1 Hierarchy From testCoroutine --> ");
            Console.WriteLine(btree1.getTreeAsString(" --> "));

            int numSuccess = btree1.CountAllForStatus(BehaviourTreeStatus.Success);
            int numFailure = btree1.CountAllForStatus(BehaviourTreeStatus.Failure);
            int numRunning = btree1.CountAllForStatus(BehaviourTreeStatus.Running);
            int numInitial = btree1.CountAllForStatus(BehaviourTreeStatus.Initial);
            
            Log("numSuccess=" + numSuccess + " numFailure=" + numFailure + " numRunning=" + numRunning + " numInitial="+numInitial);
            // numSuccess = 11 numFailure = 5 numRunning = 0 numInitial = 0
            Assert.Equal(numSuccess, 11);
            Assert.Equal(numFailure, 5);
            Assert.Equal(numRunning, 0);
            Assert.Equal(numInitial, 0);

            // Check Node Properties
            Dictionary<string, IBehaviourTreeNode> nodeMap = btree1.getNodeMap();
            Assert.True(nodeMap[par1Action3].isSuccess());
            Assert.True(nodeMap[par1Action1].isFailed());
            Assert.True(nodeMap[seq2Action2].isSuccess());
            Assert.True(nodeMap[sel1Condition].isSuccess());

            Assert.Equal(nodeMap[seq2Action2].name, seq2Action2);
            Assert.Equal(nodeMap[par1Action1].name, par1Action1);
            Assert.Equal(nodeMap[seq1Action1].name, seq1Action1);

            // Reset all node statuses to Initial
            btree1.SetStatusAll(BehaviourTreeStatus.Initial);
            Assert.Equal(btree1.CountAllForStatus(BehaviourTreeStatus.Initial), 16);
            Console.WriteLine(" ===> After Reinitializing Btree1 Hierarchy From testCoroutine --> ");
            Console.WriteLine(btree1.getTreeAsString(" --> "));

        }

        public void Log(string msg)
        {
            Console.WriteLine(msg);
        }

        IEnumerator<BehaviourTreeStatus> coroutine(string inputval, System.Action<BehaviourTreeStatus, string> callback)
        {
            Log("Beginning of coroutine()");
            float deltaTime = 315.976f;
            TimeData timeData = new TimeData(deltaTime);
            
            for (var e = btree1.Tick(timeData); e.MoveNext();)
            {
                yield return e.Current;
                callback(e.Current, "btree1");
            }
            Log("End of coroutine()");
           
        }
       
        void initTree1()
        {
            Console.WriteLine("Initializing Behavior Tree 1");

            btree1 = treeBuilder
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
                            .Do(sel1Action2, t => { return actionFail(t, sel1Action2); })
                            .Do(sel1Action3, t => { return actionFail(t, sel1Action3); })

                    .Parallel("Parallel1", 1, 1).
                        Do(par1Action1, t =>
                        {
                            return actionFail(t, par1Action1);
                        })
                        .Do(par1Action2, t =>
                        {
                            return actionFail(t, par1Action2);
                        })
                        .Do(par1Action3, t =>
                        {
                            return actionSuccess(t, par1Action3);
                        })
                    .End()
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
        public IEnumerator<BehaviourTreeStatus> actionSuccess(TimeData t, string aValue)
        {
            Console.WriteLine(aValue + " --> Action Successful ! at Delta time:" + t.deltaTime);
            callData.Add(aValue + t.deltaTime, FactionSuccess);
            Console.WriteLine("Return Status Running from actionSuccess ! - coroutine will return and then restart after this point.");
            yield return BehaviourTreeStatus.Running;
            int milliseconds = 100;
            Thread.Sleep(milliseconds);
            yield return BehaviourTreeStatus.Success;
        }
        public IEnumerator<BehaviourTreeStatus> actionFail(TimeData t, string aValue)
        {
            Console.WriteLine(aValue + " --> Action Failed ! at Delta time:" + t.deltaTime);
            //throw new ApplicationException("Node Failure to Execute !!");
            callData.Add(aValue + t.deltaTime, FactionFail);
            Console.WriteLine("Return Status Running from actionFail ! - coroutine will return and then restart after this point.");
            yield return BehaviourTreeStatus.Running;
            int milliseconds = 100;
            Thread.Sleep(milliseconds);
            yield return BehaviourTreeStatus.Failure;
        }


    }
}
