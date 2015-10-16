using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace fluent_behaviour_tree
{
    /// <summary>
    /// Interface for behaviour tree nodes.
    /// </summary>
    public interface IBehaviourTreeNode
    {
        /// <summary>
        /// Update the time of the behaviour tree.
        /// </summary>
        BehaviourTreeStatus Tick(TimeData time);
    }
}
