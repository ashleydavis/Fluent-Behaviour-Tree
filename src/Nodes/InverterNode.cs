using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentBehaviourTree
{
    /// <summary>
    /// Decorator node that inverts the success/failure of its child.
    /// </summary>
    public class InverterNode<T> : ParentBehaviourTreeNode<T>
    {
        /// <summary>
        /// Name of the node.
        /// </summary>
        private string name;

        /// <summary>
        /// The child to be inverted.
        /// </summary>
        private IBehaviourTreeNode<T> childNode;

        public InverterNode(string name)
        {
            this.name = name;
        }

        public InverterNode(string name, params IBehaviourTreeNode<T>[] behaviours): this(name)
        {
            AddChildren(behaviours);
        }

        public override BehaviourTreeStatus Tick(T time)
        {
            if (childNode == null)
            {
                throw new ApplicationException("InverterNode must have a child node!");
            }

            var result = childNode.Tick(time);
            if (result == BehaviourTreeStatus.Failure)
            {
                return BehaviourTreeStatus.Success;
            }
            else if (result == BehaviourTreeStatus.Success)
            {
                return BehaviourTreeStatus.Failure;
            }
            else
            {
                return result;
            }
        }

        /// <summary>
        /// Add a child to the parent node.
        /// </summary>
        public override void AddChild(IBehaviourTreeNode<T> child)
        {
            if (this.childNode != null)
            {
                throw new ApplicationException("Can't add more than a single child to InverterNode!");
            }

            this.childNode = child;
        }
    }
}
