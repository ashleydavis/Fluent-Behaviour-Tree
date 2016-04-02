using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentBehaviourTree { 
    public class BaseNode : IEnumerable
    {
        public BehaviourTreeStatus currentStatus { get; set; }
       
        public BaseNode() : base() {
            currentStatus = BehaviourTreeStatus.Initial;
        }
        public bool isComplete()
        {
            return currentStatus == BehaviourTreeStatus.Success || currentStatus == BehaviourTreeStatus.Failure;
        }
        public bool isRunning()
        {
            return currentStatus == BehaviourTreeStatus.Running;
        }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
