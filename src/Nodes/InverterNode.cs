using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentBehaviourTree
{
    /// <summary>
    /// Decorator node that inverts the success/failure of its child.
    /// </summary>
    public class InverterNode : BaseNode, IParentBehaviourTreeNode
    {
        /// <summary>
        /// Name of the node.
        /// </summary>
        private string name;

        /// <summary>
        /// The child to be inverted.
        /// </summary>
        private IBehaviourTreeNode childNode;

        public InverterNode(string name)
        {
            this.name = name;
        }

        public IEnumerator<BehaviourTreeStatus> Tick(TimeData time)
        {
            if (childNode == null)
            {
                throw new ApplicationException("InverterNode must have a child node!");
            }

            var result = childNode.Tick(time);
            result.MoveNext();
            if (result.Current == BehaviourTreeStatus.Failure)
            {
               yield return BehaviourTreeStatus.Success;
               yield break;
            }
            else if (result.Current == BehaviourTreeStatus.Success)
            {
               yield return BehaviourTreeStatus.Failure;
               yield break;
            }
            else
            {
                yield return result.Current;
                yield break;
            }
        }

        /// <summary>
        /// Add a child to the parent node.
        /// </summary>
        public void AddChild(IBehaviourTreeNode child)
        {
            if (this.childNode != null)
            {
                throw new ApplicationException("Can't add more than a single child to InverterNode!");
            }

            this.childNode = child;
        }
    }
}
