﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentBehaviourTree
{
    /// <summary>
    /// Runs childs nodes in parallel.
    /// </summary>
    public class ParallelNode<T> : ParentBehaviourTreeNode<T>
    {
        /// <summary>
        /// Name of the node.
        /// </summary>
        private string name;

        /// <summary>
        /// List of child nodes.
        /// </summary>
        private List<IBehaviourTreeNode<T>> children = new List<IBehaviourTreeNode<T>>();

        /// <summary>
        /// Number of child failures required to terminate with failure.
        /// </summary>
        private int numRequiredToFail;

        /// <summary>
        /// Number of child successess require to terminate with success.
        /// </summary>
        private int numRequiredToSucceed;

        public ParallelNode(string name, int numRequiredToFail, int numRequiredToSucceed)
        {
            this.name = name;
            this.numRequiredToFail = numRequiredToFail;
            this.numRequiredToSucceed = numRequiredToSucceed;
        }

        public ParallelNode(string name, int numRequiredToFail, int numRequiredToSucceed, params IBehaviourTreeNode<T>[] behaviours)
            : this(name, numRequiredToFail, numRequiredToSucceed)
        {
            AddChildren(behaviours);
        }


        public override BehaviourTreeStatus Tick(T time)
        {
            var numChildrenSuceeded = 0;
            var numChildrenFailed = 0;

            foreach (var child in children)
            {
                var childStatus = child.Tick(time);
                switch (childStatus)
                {
                    case BehaviourTreeStatus.Success: ++numChildrenSuceeded; break;
                    case BehaviourTreeStatus.Failure: ++numChildrenFailed; break;
                }
            }

            if (numRequiredToSucceed > 0 && numChildrenSuceeded >= numRequiredToSucceed)
            {
                return BehaviourTreeStatus.Success;
            }

            if (numRequiredToFail > 0 && numChildrenFailed >= numRequiredToFail)
            {
                return BehaviourTreeStatus.Failure;
            }

            return BehaviourTreeStatus.Running;
        }

        public override void AddChild(IBehaviourTreeNode<T> child)
        {
            children.Add(child);
        }
    }
}
