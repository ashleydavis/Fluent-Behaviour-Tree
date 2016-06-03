namespace FluentBehaviourTree
{
    using System.Collections.Generic;

    public class PrioritySelectorNode : PrioritySelectorNode<TimeData>
    {
        public PrioritySelectorNode(string name) : base(name)
        {
        }
    }
    /// <summary>
    /// Selects the first node that succeeds. Tries successive nodes until it finds one that doesn't fail.
    /// </summary>
    public class PrioritySelectorNode<TTickData> : IParentBehaviourTreeNode<TTickData>
    {
        /// <summary>
        /// The name of the node.
        /// </summary>
        private string name;

        /// <summary>
        /// List of child nodes.
        /// </summary>
        private List<IBehaviourTreeNode<TTickData>> children = new List<IBehaviourTreeNode<TTickData>>(); //todo: optimization, bake this to an array.

        public PrioritySelectorNode(string name)
        {
            this.name = name;
        }

        public BehaviourTreeStatus Tick(TTickData time)
        {
            foreach (var child in children)
            {
                var childStatus = child.Tick(time);
                if (childStatus != BehaviourTreeStatus.Failure)
                {
                    return childStatus;
                }
            }

            return BehaviourTreeStatus.Failure;
        }

        /// <summary>
        /// Add a child node to the selector.
        /// </summary>
        public void AddChild(IBehaviourTreeNode<TTickData> child)
        {
            children.Add(child);
        }
    }
}
