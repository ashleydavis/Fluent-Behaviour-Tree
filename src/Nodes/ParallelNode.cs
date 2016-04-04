using System.Collections;
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
        private List<IEnumerator> childStatus;
        private List<IBehaviourTreeNode> runningNodes;

        public ParallelNode(string name, int numRequiredToFail, int numRequiredToSucceed):base(name)
        {
            this.numRequiredToFail = numRequiredToFail;
            this.numRequiredToSucceed = numRequiredToSucceed;

        }

        public IEnumerator<BehaviourTreeStatus> Tick(TimeData time)
        {
            currentStatus = BehaviourTreeStatus.Running;
            childStatus = new List<IEnumerator>();
            runningNodes = new List<IBehaviourTreeNode>();
            numChildrenSuceeded = 0;
            numChildrenFailed   = 0;
            while (isRunning())
            {
                // start running each child node
                // if child node returns that it is in running state
                // then continue to the next child node until all child nodes
                // have been started. Save the ones that are running in childStatus list for 
                // advancement after all nodes have started ticking.
                foreach (IBehaviourTreeNode child in children)
                {
                    IEnumerator<BehaviourTreeStatus> chStatus = child.Tick(time);
                    chStatus.MoveNext();

                    if (chStatus.Current == BehaviourTreeStatus.Running)
                    {
                        // save Enumerator for next round of advancement after all child nodes have
                        // been started.
                        runningNodes.Add(child);
                        childStatus.Add(chStatus);
                        yield return BehaviourTreeStatus.Running;
                      
                    }
                    else {
                        switch (chStatus.Current)
                        {
                            case BehaviourTreeStatus.Success: ++numChildrenSuceeded; break;
                            case BehaviourTreeStatus.Failure: ++numChildrenFailed; break;
                        }
                    }
                }

                // if the list of saved enumerators is not empty, advance until they have all been completed
                bool hasRunning = childStatus.Count > 0;
             
                while (hasRunning)
                {
                    hasRunning = false;
                    int curElement = -1;
                    foreach (IEnumerator<BehaviourTreeStatus> cstat in childStatus)
                    {
                        ++curElement;
                        if (runningNodes[curElement].currentStatus == BehaviourTreeStatus.Running)
                        {
                            cstat.MoveNext();
                            if (cstat.Current != BehaviourTreeStatus.Running)
                            {
                                switch (cstat.Current)
                                {
                                    case BehaviourTreeStatus.Success: ++numChildrenSuceeded; break;
                                    case BehaviourTreeStatus.Failure: ++numChildrenFailed; break;
                                }
                            }
                            else {
                                hasRunning = true;
                                yield return BehaviourTreeStatus.Running;
                            }
                        }   
                    }
                }

                // All nodes should either have returned failed or successs
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
