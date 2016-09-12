namespace FluentBehaviourTree
{
    /// <summary>
    /// Runs child nodes in sequence, until one fails.
    /// </summary>
    public class SequenceNode : ParentBehaviourTreeNode
    {
        public SequenceNode(string name) : base(name) { }

        protected override BehaviourTreeStatus AbstractTick(TimeData time)
        {
            for (int i = 0; i < childCount; i++)
            {
                var child = this[i];
                var childStatus = child.Tick(time);
                if (childStatus != BehaviourTreeStatus.Success)
                {
                    return childStatus;
                }
            }
            return BehaviourTreeStatus.Success;
        }
    }
}
