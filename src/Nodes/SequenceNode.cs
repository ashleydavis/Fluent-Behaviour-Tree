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
            foreach (var child in children)
            {
                var childStatus = child.Tick(time);
                childStatus.MoveNext();
                currentStatus = childStatus.Current;

                if (isRunning())
                {
                    // keep looping until we exit running mode or we
                    // run out of enum values.
                    yield return currentStatus;
                    while (childStatus.MoveNext())
                    {
                        currentStatus = childStatus.Current;
                        if (!isRunning())
                            break;
                    }
                }
                if (isFailed())
                {
                    yield return currentStatus;
                    yield break;
                }
               
            }

            yield return currentStatus;
        }

        
    }
}
