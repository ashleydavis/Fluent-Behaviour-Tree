namespace FluentBehaviourTree
{
    using System.Collections.Generic;

    public class ParallelNode : ParallelNode<TimeData>
    {
        public ParallelNode(string name, int numRequiredToFail, int numRequiredToSucceed) : base(name, numRequiredToFail, numRequiredToSucceed)
        {
        }
    }

    /// <summary>
    /// Runs childs nodes in parallel.
    /// </summary>
    public class ParallelNode<TTickData> : IParentBehaviourTreeNode<TTickData>
    {
        /// <summary>
        /// Name of the node.
        /// </summary>
        private string name;

        /// <summary>
        /// List of child nodes.
        /// </summary>
        private List<IBehaviourTreeNode<TTickData>> children = new List<IBehaviourTreeNode<TTickData>>();

        /// <summary>
        /// Number of child failures required to terminate with failure.
        /// </summary>
        private int numRequiredToFail;

        /// <summary>
        /// Number of child successess require to terminate with success.
        /// </summary>
        private int numRequiredToSucceed;

        public ParallelNode(string name, int numRequiredToFail, int numRequiredToSucceed)
        {
            this.name = name;
            this.numRequiredToFail = numRequiredToFail;
            this.numRequiredToSucceed = numRequiredToSucceed;
        }

        public BehaviourTreeStatus Tick(TTickData time)
        {
            var numChildrenSuceeded = 0;
            var numChildrenFailed = 0;

            foreach (var child in children)
            {
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

        public void AddChild(IBehaviourTreeNode<TTickData> child)
        {
            children.Add(child);
        }
    }
}
