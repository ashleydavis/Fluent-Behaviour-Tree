using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentBehaviourTree
{
    /// <summary>
    /// A behaviour tree leaf node for running an action.
    /// </summary>
    public class ConditionNode : IBehaviourTreeNode
    {
        /// <summary>
        /// The name of the node.
        /// </summary>
        private string name;

        /// <summary>
        /// Function to invoke for the action.
        /// </summary>
        private Func<TimeData, bool> fn;

        public ConditionNode(string name, Func<TimeData, bool> fn)
        {
            this.name = name;
            this.fn = fn; 
        }
        
        public BehaviourTreeStatus Tick(TimeData time)
        {
            return fn(time) ? BehaviourTreeStatus.Success : BehaviourTreeStatus.Failure;
        }
       
    }
}
