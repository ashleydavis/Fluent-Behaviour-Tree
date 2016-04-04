using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentBehaviourTree
{
    /// <summary>
    /// The return type when invoking behaviour tree nodes.
    /// </summary>
    public enum BehaviourTreeStatus
    {
        Initial,
        Success,
        Failure,
        Running
    }
    /// <summary>
    /// Returns the BehaviourTreeStatus wrapped into an IEnumerator suitable for iterating
    /// to the next value using the MoveNext() method.
    /// </summary>
    public class TreeStatus
    {
        public static IEnumerator<BehaviourTreeStatus> getStatus(BehaviourTreeStatus aStatus)
        {
            List<BehaviourTreeStatus> onestatus = new List<BehaviourTreeStatus>();
            onestatus.Add(aStatus);
            return onestatus.GetEnumerator();
        }
        public static IEnumerator<BehaviourTreeStatus> getStatus(BehaviourTreeStatus[] status)
        {
            List<BehaviourTreeStatus> mstatus = new List<BehaviourTreeStatus>();
            foreach (var stat in mstatus)
            {
                mstatus.Add(stat);
            }
            return mstatus.GetEnumerator();
        }
    }
}
