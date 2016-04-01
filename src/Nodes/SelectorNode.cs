using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentBehaviourTree
{
    /// <summary>
    /// Selects the first node that succeeds. Tries successive nodes until it finds one that doesn't fail.
    /// </summary>
    public class SelectorNode : IParentBehaviourTreeNode
    {
        /// <summary>
        /// The name of the node.
        /// </summary>
        private string name;

        /// <summary>
        /// List of child nodes.
        /// </summary>
        private List<IBehaviourTreeNode> children = new List<IBehaviourTreeNode>(); //todo: optimization, bake this to an array.

        public SelectorNode(string name)
        {
            this.name = name;
        }

        // A Selector may be contaminated with other nodes (Condition) or IParentBeahviourTreeNodes over which
        // it's behaviour as a selector is applied. The assumption here is that any item in it's child list
        // should have  selector behaviour applied unless the first one is a condition Action.
        public BehaviourTreeStatus Tick(TimeData time)
        {
            
            int skipOne = 0;
            // check if this is a conditional action
            IBehaviourTreeNode firstNode = children[0];
            if (firstNode is ActionNode)
            {
                ActionNode firstChild = (ActionNode) firstNode;
                if (firstChild.isCondition())
                {
                    var status = firstChild.Tick(time);
                    if (status != BehaviourTreeStatus.Success)
                        return BehaviourTreeStatus.Success;
                    ++skipOne;
                }
            }
            
            foreach (var child in children.Skip(skipOne))
            {
                var childStatus = child.Tick(time);
                if (childStatus != BehaviourTreeStatus.Failure)
                {
                    return childStatus;
                }
            }

            return BehaviourTreeStatus.Failure;
        }

        /// <summary>
        /// Add a child node to the selector.
        /// </summary>
        public void AddChild(IBehaviourTreeNode child)
        {
            children.Add(child);
        }
    }
}
