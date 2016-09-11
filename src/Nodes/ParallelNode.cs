namespace FluentBehaviourTree
{
    /// <summary>
    /// Runs childs nodes in parallel.
    /// </summary>
    public class ParallelNode : ParentBehaviourTreeNode
    {
        /// <summary>
        /// Number of child failures required to terminate with failure.
        /// </summary>
        private int numRequiredToFail;
        /// <summary>
        /// Number of child successess require to terminate with success.
        /// </summary>
        private int numRequiredToSucceed;

        public ParallelNode(string name, int numRequiredToFail, int numRequiredToSucceed) : base(name)
        {
            this.numRequiredToFail = numRequiredToFail;
            this.numRequiredToSucceed = numRequiredToSucceed;
        }

        protected override BehaviourTreeStatus AbstractTick(TimeData time)
        {
            var numChildrenSuceeded = 0;
            var numChildrenFailed = 0;

            for (int i = 0; i < childCount; i++)
            {
                var child = this[i];
                var childStatus = child.Tick(time);
                switch (childStatus)
                {
                    case BehaviourTreeStatus.Success: ++numChildrenSuceeded; break;
                    case BehaviourTreeStatus.Failure: ++numChildrenFailed; break;
                }
            }
            if (numRequiredToSucceed > 0 && numChildrenSuceeded >= numRequiredToSucceed)
            {
                return BehaviourTreeStatus.Success;
            }
            if (numRequiredToFail > 0 && numChildrenFailed >= numRequiredToFail)
            {
                return BehaviourTreeStatus.Failure;
            }
            return BehaviourTreeStatus.Running;
        }
    }
}
