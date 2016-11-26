using System.Collections.Generic;

namespace FluentBehaviourTree
{
    /// <summary>
    /// Runs child nodes in sequence, until one fails.
    /// </summary>
    public class SequenceNode : BaseParentNode, IParentBehaviourTreeNode
    {
      
        public SequenceNode(string name):base(name)
        {
        }

        public IEnumerator<BehaviourTreeStatus> Tick(TimeData time)
        {
            currentStatus = BehaviourTreeStatus.Running;
            int numFailed = 0;
            foreach (var child in children)
            {
                var childStatus = child.Tick(time);
                childStatus.MoveNext();
                currentStatus = childStatus.Current;
                if (isRunning())
                {
                    // keep looping until we exit running mode or we
                    // run out of enum values.
                    yield return BehaviourTreeStatus.Running;
                    while (childStatus.MoveNext())
                    {
                        currentStatus = childStatus.Current;
                        if (isComplete())
                            break;
                    }
                    // if node is still running and has run out of values
                    // this is an error - should always return success/failure
                    // eventually.
                    if (isRunning())
                    {
                        child.currentStatus = BehaviourTreeStatus.Failure;
                        currentStatus       = BehaviourTreeStatus.Failure;
                    }
                }
                if (isFailed())
                {
                    break;
                }
               
            }

            yield return currentStatus;
        }

        
    }
}
