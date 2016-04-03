using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentBehaviourTree
{
    public class BaseParentNode : BaseNode
    {
        /// <summary>
        /// List of child nodes.
        /// </summary>
        protected List<IBehaviourTreeNode> children = new List<IBehaviourTreeNode>();
        public BaseParentNode(string aName) : base(aName)
        {
        }
        /// <summary>
        /// Add a child to the sequence and set pointer to it's parent so we can get
        /// to the parent from the child (if necessary)
        /// </summary>
        public void AddChild(IBehaviourTreeNode child)
        {
            child.parent = (IBehaviourTreeNode) this;
            children.Add(child);
        }
       
        public override void SetStatusAll(BehaviourTreeStatus aStatus)
        {
            currentStatus = aStatus;
            foreach (var child in children)
            {
                child.SetStatusAll(aStatus);
            }
        }
        public override int CountAllForStatus(BehaviourTreeStatus aStatus)
        {
            // return a count of the status for all nodes under the current node that have the 
            // specified status value
            int numCount = 0;
            if (currentStatus == aStatus) ++numCount;
            foreach (var child in children)
            {
               numCount += child.CountAllForStatus(aStatus);
            }
            return numCount;
        }
        public override string getTreeAsString(string prefix)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(base.getTreeAsString(prefix));
            prefix += treePrefix;
            foreach (var child in children) {
                builder.Append(child.getTreeAsString(prefix));
            }
            return builder.ToString();
        }
    }
}
