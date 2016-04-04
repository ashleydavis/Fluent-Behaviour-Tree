using System.Collections.Generic;

namespace FluentBehaviourTree
{
    /// <summary>
    /// Runs childs nodes in parallel.
    /// </summary>
    public class ParallelNode : BaseParentNode, IParentBehaviourTreeNode
    {
       
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

        public ParallelNode(string name, int numRequiredToFail, int numRequiredToSucceed):base(name)
        {
            this.numRequiredToFail = numRequiredToFail;
            this.numRequiredToSucceed = numRequiredToSucceed;

        }

        public IEnumerator<BehaviourTreeStatus> Tick(TimeData time)
        {
           
            currentStatus = BehaviourTreeStatus.Running;

            while (isRunning())
            {
                numChildrenSuceeded = 0;
                numChildrenFailed = 0;

                foreach (var child in children)
                {
                    var childStatus = child.Tick(time);
                    childStatus.MoveNext();

                    if (childStatus.Current == BehaviourTreeStatus.Running)
                    {
                        // keep looping until we exit running mode or we
                        // run out of enum values.
                        yield return childStatus.Current;
                        while (childStatus.MoveNext())
                        {
                            if (childStatus.Current != BehaviourTreeStatus.Running)
                                break;
                            yield return childStatus.Current;
                        }
                    }
                    switch (childStatus.Current)
                    {
                        case BehaviourTreeStatus.Success: ++numChildrenSuceeded; break;
                        case BehaviourTreeStatus.Failure: ++numChildrenFailed; break;
                    }
                }

                if (numRequiredToSucceed > 0 && numChildrenSuceeded >= numRequiredToSucceed)
                {
                    currentStatus = BehaviourTreeStatus.Success;
                }
                else
                if (numRequiredToFail > 0 && numChildrenFailed >= numRequiredToFail)
                {
                    currentStatus = BehaviourTreeStatus.Failure;
                }
                else {
                    // Return Running if conditions were not met
                    currentStatus = BehaviourTreeStatus.Running;
                }
                yield return currentStatus;
            }
        }
        
    }
}
