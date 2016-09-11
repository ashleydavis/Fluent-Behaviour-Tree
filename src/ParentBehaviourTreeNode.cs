using System.Collections.Generic;

namespace FluentBehaviourTree
{
    /// <summary>
    /// Interface for behaviour tree nodes.
    /// </summary>
    public abstract class ParentBehaviourTreeNode : BehaviourTreeNode
    {
        /// <summary>
        /// List of child nodes used for building nodes.
        /// </summary>
        private List<BehaviourTreeNode> _children = new List<BehaviourTreeNode>();
        /// <summary>
        /// The baked array of children.
        /// </summary>
        private BehaviourTreeNode[] children = new BehaviourTreeNode[1];

        public int childCount {
            get {
                if (hasDataBeenBaked)
                {
                    return children.Length;
                }
                return _children.Count;
            }
        }

        public ParentBehaviourTreeNode(string name) : base(name) { }

        public BehaviourTreeNode this[int index] {
            get {
                if (hasDataBeenBaked)
                {
                    return children[index];
                }
                return _children[index];
            }
        }

        /// <summary>
        /// Add a child to the parent node.
        /// </summary>
        public virtual void AddChild(BehaviourTreeNode child)
        {
            if (!hasDataBeenBaked)
            {
                _children.Add(child);
            }
        }

        protected override void InternalBakeData()
        {
            base.InternalBakeData();
            children = _children.ToArray();
            _children.Clear();
        }
    }
}
