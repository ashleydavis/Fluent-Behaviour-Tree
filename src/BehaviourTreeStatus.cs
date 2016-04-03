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
    public class TreeStatus
    {
        public static IEnumerator<BehaviourTreeStatus> getStatus(BehaviourTreeStatus aStatus)
        {
            List<BehaviourTreeStatus> status = new List<BehaviourTreeStatus>();
            status.Add(aStatus);
            return status.GetEnumerator();
        }
    }
}
