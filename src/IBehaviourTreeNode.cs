namespace FluentBehaviourTree
{
    public interface IBehaviourTreeNode : IBehaviourTreeNode<TimeData>
    { }

    /// <summary>
    /// Interface for behaviour tree nodes.
    /// </summary>
    public interface IBehaviourTreeNode<TTickData>
    {
        /// <summary>
        /// Update the time of the behaviour tree.
        /// </summary>
        BehaviourTreeStatus Tick(TTickData time);
    }
}
