using System;

namespace FluentBehaviourTree
{
    /// <summary>
    /// A behaviour tree leaf node for running an action.
    /// </summary>
    public class ActionNode : BehaviourTreeNode
    {
        /// <summary>
        /// Function to invoke for the action.
        /// </summary>
        private Func<TimeData, BehaviourTreeStatus> fn;

        public ActionNode(string name, Func<TimeData, BehaviourTreeStatus> fn) : base(name)
        {
            this.fn = fn;
        }

        protected override BehaviourTreeStatus AbstractTick(TimeData time)
        {
            return fn(time);
        }
    }
}
