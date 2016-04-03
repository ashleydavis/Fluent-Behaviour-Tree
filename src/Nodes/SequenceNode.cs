using System.Collections.Generic;

namespace FluentBehaviourTree
{
    /// <summary>
    /// Runs child nodes in sequence, until one fails.
    /// </summary>
    public class SequenceNode : BaseNode, IParentBehaviourTreeNode
    {
        /// <summary>
        /// Name of the node.
        /// </summary>
        private string name;

        /// <summary>
        /// List of child nodes.
        /// </summary>
        private List<IBehaviourTreeNode> children = new List<IBehaviourTreeNode>(); //todo: this could be optimized as a baked array.

        public SequenceNode(string name)
        {
            this.name = name;
        }

        public IEnumerator<BehaviourTreeStatus> Tick(TimeData time)
        {
            foreach (var child in children)
            {
                var childStatus = child.Tick(time);
                childStatus.MoveNext();
                currentStatus = childStatus.Current;
                if (isFailed())
                {
                    yield return currentStatus;
                    yield break;
                }
                else if (isRunning())
                {
                    yield return currentStatus;
                    while (childStatus.MoveNext())
                    {
                        currentStatus = childStatus.Current;
                        if (isComplete())
                        {
                            yield return currentStatus;
                            yield break;
                        }
                    }
                    // if failed exit and return status
                    if (isFailed())
                    {
                        yield return currentStatus;
                        yield break;
                    }
                }
            }

            yield return currentStatus;
        }

        /// <summary>
        /// Add a child to the sequence.
        /// </summary>
        public void AddChild(IBehaviourTreeNode child)
        {
            children.Add(child);
        }
    }
}
