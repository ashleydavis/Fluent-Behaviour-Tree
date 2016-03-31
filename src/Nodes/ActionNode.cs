using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentBehaviourTree
{
    /// <summary>
    /// A behaviour tree leaf node for running an action.
    /// </summary>
    public class ActionNode : IBehaviourTreeNode
    {
        /// <summary>
        /// The name of the node.
        /// </summary>
        private string name;
        // only set if action is from a condition node
        private bool condition;

        /// <summary>
        /// Function to invoke for the action.
        /// </summary>
        private Func<TimeData, BehaviourTreeStatus> fn;
        

        public ActionNode(string name, Func<TimeData, BehaviourTreeStatus> fn)
        {
            this.name=name;
            this.fn=fn;
            this.condition = false;
        }

        public ActionNode(string name, Func<TimeData, BehaviourTreeStatus> fn, bool cond)
        {
            this.name = name;
            this.fn = fn;
            this.condition = cond;
        }

        public BehaviourTreeStatus Tick(TimeData time)
        {
            return fn(time);
        }
        public bool isCondition()
        {
            return condition;
        }
    }
}
