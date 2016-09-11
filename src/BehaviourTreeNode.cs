namespace FluentBehaviourTree
{
    /// <summary>
    /// Interface for behaviour tree nodes.
    /// </summary>
    public abstract class BehaviourTreeNode
    {
        /// <summary>
        /// The reference name for this node
        /// </summary>
        public string name { get; private set; }
        /// <summary>
        /// The result of the last execution
        /// </summary>
        public BehaviourTreeStatus lastExecutionStatus { get; private set; }
        protected bool hasDataBeenBaked { get; private set; }

        public BehaviourTreeNode(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// Update the time of the behaviour tree.
        /// </summary>
        public BehaviourTreeStatus Tick(TimeData time)
        {
            lastExecutionStatus = AbstractTick(time);
            return lastExecutionStatus;
        }
        internal void BakeData()
        {
            if (!hasDataBeenBaked)
            {
                InternalBakeData();
                hasDataBeenBaked = true;
            }
        }
        protected virtual void InternalBakeData() { }
        protected abstract BehaviourTreeStatus AbstractTick(TimeData time);
    }
}
