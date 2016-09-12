using System.Collections.Generic;

namespace FluentBehaviourTree
{
    /// <summary>
    /// Interface for behaviour tree nodes.
    /// </summary>
    public abstract class ParentBehaviourTreeNode : BehaviourTreeNode
    {
        /// <summary>
        /// List of child nodes
        /// </summary>
        private List<BehaviourTreeNode> children = new List<BehaviourTreeNode>();

        /// <summary>
        /// The number of children added to this node
        /// </summary>
        public int childCount {
            get {
                return children.Count;
            }
        }

        public ParentBehaviourTreeNode(string name) : base(name) { }

        /// <summary>
        /// Retrieve a child node by index.
        /// </summary>
        public BehaviourTreeNode this[int index] {
            get {
                return children[index];
            }
        }
        /// <summary>
        /// Marks that this node and all children have not execute yet.
        /// </summary>
        internal override void ResetLastExecStatus()
        {
            base.ResetLastExecStatus();
            for (int i = 0; i < childCount; i++)
            {
                this[i].ResetLastExecStatus();
            }
        }
        /// <summary>
        /// Add a child to the parent node.
        /// </summary>
        public virtual void AddChild(BehaviourTreeNode child)
        {
            children.Add(child);
        }
    }
}
