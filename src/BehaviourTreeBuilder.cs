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
        private IBehaviourTreeNode curNode;

        /// <summary>
        /// Create an action node.
        /// </summary>
        public BehaviourTreeBuilder Do(string name, Func<TimeData, BehaviourTreeStatus> fn)
        {
            curNode = new ActionNode(name, fn);
            return this;
        }

        /// <summary>
        /// Build the actual tree.
        /// </summary>
        public IBehaviourTreeNode Build()
        {
            return curNode;
        }
    }
}
