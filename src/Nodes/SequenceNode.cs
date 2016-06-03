namespace FluentBehaviourTree
{
    using System.Collections.Generic;

    public class SequenceNode : SequenceNode<TimeData>
    {
        public SequenceNode(string name) : base(name)
        {
        }
    }

    /// <summary>
    /// Runs child nodes in sequence, until one fails.
    /// </summary>
    public class SequenceNode<TTickData> : IParentBehaviourTreeNode<TTickData>
    {
        /// <summary>
        /// Name of the node.
        /// </summary>
        private string name;

        /// <summary>
        /// List of child nodes.
        /// </summary>
        private List<IBehaviourTreeNode<TTickData>> children = new List<IBehaviourTreeNode<TTickData>>(); //todo: this could be optimized as a baked array.

        public SequenceNode(string name)
        {
            this.name = name;
        }

        public BehaviourTreeStatus Tick(TTickData time)
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
        public void AddChild(IBehaviourTreeNode<TTickData> child)
        {
            children.Add(child);
        }
    }
}
