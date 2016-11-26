using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentBehaviourTree { 
    public class BaseNode : IEnumerable
    {
        public BehaviourTreeStatus currentStatus { get; set; }
        public IBehaviourTreeNode parent { get; set; }
        public Dictionary<string,IBehaviourTreeNode> nodeMap { get; set; }
        public static readonly string treePrefix = " --> ";
        /// <summary>
        /// The name of the node.
        /// </summary>
        public string name { get; set; }

        public BaseNode(string aName) : base() {
            parent = null;
            name   = aName;
            currentStatus = BehaviourTreeStatus.Initial;
        }
        public bool isComplete()
        {
            return currentStatus == BehaviourTreeStatus.Success || currentStatus == BehaviourTreeStatus.Failure;
        }
        public bool isRunning()
        {
            return currentStatus == BehaviourTreeStatus.Running;
        }
        public bool isSuccess()
        {
            return currentStatus == BehaviourTreeStatus.Success;
        }
        public bool isFailed()
        {
            return currentStatus == BehaviourTreeStatus.Failure;
        }
        public virtual void SetStatusAll(BehaviourTreeStatus aStatus)
        {
            currentStatus = aStatus;
        }
        public bool hasParent()
        {
            return parent != null;
        }
        public virtual int CountAllForStatus(BehaviourTreeStatus aStatus)
        {
            if (currentStatus == aStatus)
                return 1;
            else
                return 0;
        }
        public Dictionary<string,IBehaviourTreeNode> getNodeMap()
        {
            if (hasParent())
                return parent.getNodeMap();
            else
                return this.nodeMap;
        }
        public IBehaviourTreeNode getNode(string aNodeName)
        {
            return getNodeMap()[aNodeName];
        }
        public virtual string getTreeAsString(string prefix)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(prefix + name + "[" + this.GetType() + "] - " + currentStatus+  "\n");
            return builder.ToString();
        }
        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
