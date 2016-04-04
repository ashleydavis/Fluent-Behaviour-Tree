﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentBehaviourTree
{
    /// <summary>
    /// Selects the first node that succeeds. Tries successive nodes until it finds one that doesn't fail.
    /// </summary>
    public class SelectorNode : BaseParentNode, IParentBehaviourTreeNode
    {
       
        private bool isRandom;

        public SelectorNode(string name, bool random=false):base(name)
        {
            this.isRandom = random;
        }

        // A Selector may be contaminated with other nodes (Condition) or IParentBeahviourTreeNodes over which
        // it's behaviour as a selector is applied. The assumption here is that any item in it's child list
        // should have  selector behaviour applied unless the first one is a condition Action.
        public IEnumerator<BehaviourTreeStatus> Tick(TimeData time)
        {
            currentStatus   = BehaviourTreeStatus.Running;
            int skipOne     = 0;
            int numSuccess  = 0;
            int numFailed   = 0;
            // check if this is a conditional action
            IBehaviourTreeNode firstNode = children[0];
            if (firstNode is ConditionNode)
            {
                ConditionNode firstChild = (ConditionNode) firstNode;
                var status = firstChild.Tick(time);
                status.MoveNext();

                if (status.Current != BehaviourTreeStatus.Success)
                {
                    // Return Failure and Exit the Node
                    currentStatus = BehaviourTreeStatus.Failure;
                    yield return currentStatus;
                    yield break;
                } else
                    ++skipOne; 
            }

            List<IBehaviourTreeNode> remainingChildren = children.Skip(skipOne).ToList<IBehaviourTreeNode>();
            if (isRandom)
                remainingChildren = Randomize(remainingChildren);

            foreach (var child in remainingChildren)
            {
                var childStatus = child.Tick(time);
                childStatus.MoveNext();
               
                if (childStatus.Current == BehaviourTreeStatus.Running)
                {
                    // keep looping until we exit running mode or we
                    // run out of enum values.
                    yield return BehaviourTreeStatus.Running;
                    while (childStatus.MoveNext())
                    {                      
                        if (childStatus.Current != BehaviourTreeStatus.Running)
                            break;
                    }
                    // if node is still running and has run out of values
                    // this is an error - should always return success/failure
                    // eventually.
                    if (childStatus.Current == BehaviourTreeStatus.Running)
                    {
                        child.currentStatus = BehaviourTreeStatus.Failure;
                    }
                }
                // On exit above, status should be success or fail 
                // if success, then exit selector
                if (childStatus.Current == BehaviourTreeStatus.Success)
                {
                    ++numSuccess;
                      break;
                } else  ++numFailed;
              
            }
            if (numSuccess > 0)
                currentStatus = BehaviourTreeStatus.Success;
            else
                currentStatus = BehaviourTreeStatus.Failure;

            yield return currentStatus;
            yield break;
        }

        // randomize a list
        public static List<T> Randomize<T>(List<T> list)
        {
            List<T> randomizedList = new List<T>();
            Random rnd = new Random();
            while (list.Count > 0)
            {
                int index = rnd.Next(0, list.Count); //pick a random item from the master list
                randomizedList.Add(list[index]); //place it at the end of the randomized list
                list.RemoveAt(index);
            }
            return randomizedList;
        }

    }
}
