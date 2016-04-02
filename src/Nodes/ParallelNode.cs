using System.Collections.Generic;

namespace FluentBehaviourTree
{
    /// <summary>
    /// Runs childs nodes in parallel.
    /// </summary>
    public class ParallelNode : BaseNode, IParentBehaviourTreeNode
    {
        /// <summary>
        /// Name of the node.
        /// </summary>
        private string name;

        /// <summary>
        /// List of child nodes.
        /// </summary>
        private List<IBehaviourTreeNode> children = new List<IBehaviourTreeNode>();

        /// <summary>
        /// Number of child failures required to terminate with failure.
        /// </summary>
        private int numRequiredToFail;

        /// <summary>
        /// Number of child successess require to terminate with success.
        /// </summary>
        private int numRequiredToSucceed;
        private int numChildrenSuceeded;
        private int numChildrenFailed;

        public ParallelNode(string name, int numRequiredToFail, int numRequiredToSucceed)
        {
            this.name = name;
            this.numRequiredToFail = numRequiredToFail;
            this.numRequiredToSucceed = numRequiredToSucceed;

        }

        public IEnumerator<BehaviourTreeStatus> Tick(TimeData time)
        {
           
            currentStatus = BehaviourTreeStatus.Running;
            while (currentStatus != BehaviourTreeStatus.Completed)
            {
                numChildrenSuceeded = 0;
                numChildrenFailed = 0;

                foreach (var child in children)
                {
                    var childStatus = child.Tick(time);
                    childStatus.MoveNext();
                    switch (childStatus.Current)
                    {
                        case BehaviourTreeStatus.Success: ++numChildrenSuceeded; break;
                        case BehaviourTreeStatus.Failure: ++numChildrenFailed; break;
                    }
                }

                if (numRequiredToSucceed > 0 && numChildrenSuceeded >= numRequiredToSucceed)
                {
                    yield return BehaviourTreeStatus.Success;
                    currentStatus = BehaviourTreeStatus.Completed;
                    yield break;
                }
                else
                if (numRequiredToFail > 0 && numChildrenFailed >= numRequiredToFail)
                {
                    yield return BehaviourTreeStatus.Failure;
                    currentStatus = BehaviourTreeStatus.Completed;
                    yield break;
                }
                else
                    yield return BehaviourTreeStatus.Running;
            }
        }

        public void AddChild(IBehaviourTreeNode child)
        {
            children.Add(child);
        }
    }
}
