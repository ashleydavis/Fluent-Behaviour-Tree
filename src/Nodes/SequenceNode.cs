using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentBehaviourTree
{
    /// <summary>
    /// Runs child nodes in sequence, until one fails.
    /// </summary>
    public class SequenceNode<T> : ParentBehaviourTreeNode<T>
    {
        /// <summary>
        /// Name of the node.
        /// </summary>
        private string name;

        /// <summary>
        /// List of child nodes.
        /// </summary>
        private List<IBehaviourTreeNode<T>> children = new List<IBehaviourTreeNode<T>>(); //todo: this could be optimized as a baked array.

        public SequenceNode(string name)
        {
            this.name = name;
        }

        public SequenceNode(string name, params IBehaviourTreeNode<T>[] behaviours): this(name)
        {
            AddChildren(behaviours);
        }

        public override BehaviourTreeStatus Tick(T time)
        {
            foreach (var child in children)
            {
                var childStatus = child.Tick(time);
                if (childStatus != BehaviourTreeStatus.Success)
                {
                    return childStatus;
                }
            }

            return BehaviourTreeStatus.Success;
        }

        /// <summary>
        /// Add a child to the sequence.
        /// </summary>
        public override void AddChild(IBehaviourTreeNode<T> child)
        {
            children.Add(child);
        }
    }
}
