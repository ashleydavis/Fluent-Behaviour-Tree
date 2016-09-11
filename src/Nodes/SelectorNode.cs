namespace FluentBehaviourTree
{
    /// <summary>
    /// Selects the first node that succeeds. Tries successive nodes until it finds one that doesn't fail.
    /// </summary>
    public class SelectorNode : ParentBehaviourTreeNode
    {
        public SelectorNode(string name) : base(name) { }

        protected override BehaviourTreeStatus AbstractTick(TimeData time)
        {
            for (int i = 0; i < childCount; i++)
            {
                var child = this[i];
                var childStatus = child.Tick(time);
                if (childStatus != BehaviourTreeStatus.Failure)
                {
                    return childStatus;
                }
            }
            return BehaviourTreeStatus.Failure;
        }
    }
}
