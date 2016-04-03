using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentBehaviourTree
{
    /// <summary>
    /// Selects the first node that succeeds. Tries successive nodes until it finds one that doesn't fail.
    /// </summary>
    public class SelectorNode : BaseNode, IParentBehaviourTreeNode
    {
        /// <summary>
        /// The name of the node.
        /// </summary>
        private string name;
        private bool isRandom;

        /// <summary>
        /// List of child nodes.
        /// </summary>
        private List<IBehaviourTreeNode> children = new List<IBehaviourTreeNode>(); //todo: optimization, bake this to an array.

        public SelectorNode(string name, bool random=false)
        {
            this.name = name;
            this.isRandom = random;
        }


        // A Selector may be contaminated with other nodes (Condition) or IParentBeahviourTreeNodes over which
        // it's behaviour as a selector is applied. The assumption here is that any item in it's child list
        // should have  selector behaviour applied unless the first one is a condition Action.
        public IEnumerator<BehaviourTreeStatus> Tick(TimeData time)
        {
             int skipOne = 0;
            // check if this is a conditional action
            IBehaviourTreeNode firstNode = children[0];
            if (firstNode is ConditionNode)
            {
                ConditionNode firstChild = (ConditionNode) firstNode;
                var status = firstChild.Tick(time);
                status.MoveNext();

                if (status.Current != BehaviourTreeStatus.Success)
                {
                    currentStatus = BehaviourTreeStatus.Failure;
                    yield return currentStatus;
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
                currentStatus = childStatus.Current;
                if (isSuccess())
                {
                    currentStatus = childStatus.Current;
                    yield return currentStatus;
                    yield break;
                }
                // keep running until completed
                else if (isRunning())
                {
                    yield return currentStatus;
                    while (childStatus.MoveNext())
                    {
                        currentStatus = childStatus.Current;
                        if (isComplete())
                        {
                            yield return currentStatus;
                            yield break;
                        }
                    }
                    // if failed exit and return status
                    if (isFailed())
                    {
                        yield return currentStatus;
                        yield break;
                    }
                }
            }

            yield return currentStatus;
        }

        /// <summary>
        /// Add a child node to the selector.
        /// </summary>
        public void AddChild(IBehaviourTreeNode child)
        {
            children.Add(child);
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
