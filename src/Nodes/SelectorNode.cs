using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace fluent_behaviour_tree.Nodes
{
    /// <summary>
    /// Selects the first node that succeeds. Tries successive nodes until it finds one that doesn't fail.
    /// </summary>
    public class SelectorNode : IBehaviourTreeNode
    {
        /// <summary>
        /// List of child nodes.
        /// </summary>
        private List<IBehaviourTreeNode> children = new List<IBehaviourTreeNode>(); //todo: optimization, bake this to an array.

        public BehaviourTreeStatus Tick(TimeData time)
        {
            foreach (var child in children)
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
