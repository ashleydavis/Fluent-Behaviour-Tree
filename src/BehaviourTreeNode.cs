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
        /// Used to determine if this node has been ticked yet this iteration.
        /// </summary>
        public bool hasExecuted { get; private set; }

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
            ResetLastExecStatus();
            lastExecutionStatus = AbstractTick(time);
            hasExecuted = true;
            return lastExecutionStatus;
        }
        /// <summary>
        /// Marks that this node hasn't executed yet.
        /// </summary>
        internal virtual void ResetLastExecStatus()
        {
            hasExecuted = false;
        }
        protected abstract BehaviourTreeStatus AbstractTick(TimeData time);
    }
}
