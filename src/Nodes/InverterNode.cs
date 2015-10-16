using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace fluent_behaviour_tree.Nodes
{
    /// <summary>
    /// Decorator node that inverts the success/failure of its child.
    /// </summary>
    public class InverterNode
    {
        /// <summary>
        /// The child to be inverted.
        /// </summary>
        private IBehaviourTreeNode childNode;

        public BehaviourTreeStatus Tick(TimeData time)
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
        /// Set the child to be inverted.
        /// </summary>
        public void SetChild(IBehaviourTreeNode childNode)
        {
            this.childNode = childNode;
        }
    }
}
