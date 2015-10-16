using fluent_behaviour_tree.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace fluent_behaviour_tree
{
    /// <summary>
    /// Fluent API for building a behaviour tree.
    /// </summary>
    public class BehaviourTreeBuilder
    {
        /// <summary>
        /// Create an action node.
        /// </summary>
        public static IBehaviourTreeNode Do(string name, Func<TimeData, BehaviourTreeStatus> fn)
        {
            return new ActionNode(name, fn);
        }
    }
}
