using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentBehaviourTree
{
    /// <summary>
    /// A behaviour tree leaf node for running an action.
    /// </summary>
    public class ActionNode : BaseNode,IBehaviourTreeNode
    {
        /// <summary>
        /// The name of the node.
        /// </summary>
        private string name;
      

        /// <summary>
        /// Function to invoke for the action.
        /// </summary>
        private Func<TimeData, IEnumerator<BehaviourTreeStatus>> fn;
        

        public ActionNode(string name, Func<TimeData, IEnumerator<BehaviourTreeStatus>> fn)
        {
            this.name=name;
            this.fn=fn;
          
        }

        public IEnumerator<BehaviourTreeStatus> Tick(TimeData time)
        {
           for ( var e = fn(time); e.MoveNext(); )
            {
                currentStatus = e.Current;
                yield return currentStatus;
                if (isComplete())
                    yield break;
            }
           
        }
      
    }
}
