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
        /// The child to be inverted.
        /// </summary>
        private IBehaviourTreeNode childNode;

        public InverterNode(string name): base(name)
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
            currentStatus = result.Current;
            if (isRunning())
            {
                // keep looping until we exit running mode or we
                // run out of enum values.
                yield return currentStatus;
                while (result.MoveNext())
                {
                    currentStatus = result.Current;
                    if (!isRunning())
                        break;
                }
            }
            if (isFailed())
            {
               currentStatus = BehaviourTreeStatus.Success;
            }
            else if (isSuccess())
            {
               currentStatus =  BehaviourTreeStatus.Failure;
            }
           
            yield return currentStatus;
           
           
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
            childNode.parent = this;
        }
        public override void SetStatusAll(BehaviourTreeStatus aStatus)
        {
            currentStatus = aStatus;
            childNode.SetStatusAll(aStatus);
        }
        public override int CountAllForStatus(BehaviourTreeStatus aStatus)
        {
            // return a count of the status for all nodes under the current node that have the 
            // specified status value
            int numCount = 0;
            if (currentStatus == aStatus) ++numCount;
            numCount += childNode.CountAllForStatus(aStatus);
            return numCount;
        }
        public override string getTreeAsString(string prefix)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(base.getTreeAsString(prefix));
            prefix += treePrefix;
            builder.Append(childNode.getTreeAsString(prefix));   
            return builder.ToString();
        }
    }
}
