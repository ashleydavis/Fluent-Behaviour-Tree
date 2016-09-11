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
        /// <summary>
        /// Used to determine if this node has been ticked.
        /// </summary>
        public bool hasExecuted { get; private set; }
        /// <summary>
        /// True if the Builder that made this node was built.
        /// </summary>
        protected bool hasDataBeenBaked { get; private set; }

        public BehaviourTreeNode(string name)
        {
            this.name = name;
            lastExecutionStatus = BehaviourTreeStatus.Failure;
        }

        /// <summary>
        /// Update the time of the behaviour tree.
        /// </summary>
        public BehaviourTreeStatus Tick(TimeData time)
        {
            lastExecutionStatus = AbstractTick(time);
            hasExecuted = true;
            return lastExecutionStatus;
        }
        public virtual void ResetLastExecStatus()
        {
            hasExecuted = false;
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
