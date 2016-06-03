namespace FluentBehaviourTree
{
    public interface IParentBehaviourTreeNode : IParentBehaviourTreeNode<TimeData>
    { }

    /// <summary>
    /// Interface for behaviour tree nodes.
    /// </summary>
    public interface IParentBehaviourTreeNode<TTickData> : IBehaviourTreeNode<TTickData>
    {
        /// <summary>
        /// Add a child to the parent node.
        /// </summary>
        void AddChild(IBehaviourTreeNode<TTickData> child);
    }
}
