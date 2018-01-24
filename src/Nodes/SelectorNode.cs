using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentBehaviourTree
{
    /// <summary>
    /// Selects the first node that succeeds. Tries successive nodes until it finds one that doesn't fail.
    /// </summary>
    public class SelectorNode<T> : ParentBehaviourTreeNode<T>
    {
        /// <summary>
        /// The name of the node.
        /// </summary>
        private string name;

        /// <summary>
        /// List of child nodes.
        /// </summary>
        private List<IBehaviourTreeNode<T>> children = new List<IBehaviourTreeNode<T>>(); //todo: optimization, bake this to an array.

        public SelectorNode(string name)
        {
            this.name = name;
        }

        public SelectorNode(string name, params IBehaviourTreeNode<T>[] behaviours):this(name)
        {
            AddChildren(behaviours);
        }

        public override BehaviourTreeStatus Tick(T time)
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
        public override void AddChild(IBehaviourTreeNode<T> child)
        {
            children.Add(child);
        }
    }
}
