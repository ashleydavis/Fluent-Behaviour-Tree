using System;

namespace FluentBehaviourTree
{
    /// <summary>
    /// Decorator node that inverts the success/failure of its child.
    /// </summary>
    public class InverterNode : ParentBehaviourTreeNode
    {
        /// <summary>
        /// The child to be inverted.
        /// </summary>
        private BehaviourTreeNode childNode {
            get {
                if (childCount == 0)
                {
                    return null;
                }
                return this[0];
            }
        }

        public InverterNode(string name) : base(name) { }

        protected override BehaviourTreeStatus AbstractTick(TimeData time)
        {
            if (childNode == null)
            {
                throw new ApplicationException("InverterNode must have a child node!");
            }

            var result = childNode.Tick(time);
            if (result == BehaviourTreeStatus.Failure)
            {
                return BehaviourTreeStatus.Success;
            }
            else if (result == BehaviourTreeStatus.Success)
            {
                return BehaviourTreeStatus.Failure;
            }
            else
            {
                return result;
            }
        }

        public override void AddChild(BehaviourTreeNode child)
        {
            if (childNode != null)
            {
                throw new ApplicationException("Can't add more than a single child to InverterNode!");
            }
            base.AddChild(child);
        }
    }
}
