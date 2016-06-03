namespace FluentBehaviourTree
{
    using System;

    public class ActionNode : ActionNode<TimeData>
    {
        public ActionNode(string name, Func<TimeData, BehaviourTreeStatus> fn) : base(name, fn)
        {
        }
    }

    /// <summary>
    /// A behaviour tree leaf node for running an action.
    /// </summary>
    public class ActionNode<TTickData> : IBehaviourTreeNode<TTickData>
    {
        /// <summary>
        /// The name of the node.
        /// </summary>
        private string name;

        /// <summary>
        /// Function to invoke for the action.
        /// </summary>
        private Func<TTickData, BehaviourTreeStatus> fn;


        public ActionNode(string name, Func<TTickData, BehaviourTreeStatus> fn)
        {
            this.name = name;
            this.fn = fn;
        }

        public BehaviourTreeStatus Tick(TTickData time)
        {
            return fn(time);
        }
    }
}
