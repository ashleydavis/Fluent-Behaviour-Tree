namespace FluentBehaviourTree
{
    using System;
    using System.Collections.Generic;

    public class BehaviourTreeBuilder : BehaviourTreeBuilder<TimeData>
    {

    }

    /// <summary>
    /// Fluent API for building a behaviour tree.
    /// </summary>
    public class BehaviourTreeBuilder<TTickData>
    {
        /// <summary>
        /// Last node created.
        /// </summary>
        private IBehaviourTreeNode<TTickData> curNode = null;

        /// <summary>
        /// Stack node nodes that we are build via the fluent API.
        /// </summary>
        private Stack<IParentBehaviourTreeNode<TTickData>> parentNodeStack = new Stack<IParentBehaviourTreeNode<TTickData>>();

        /// <summary>
        /// Create an action node.
        /// </summary>
        public BehaviourTreeBuilder<TTickData> Do(string name, Func<TTickData, BehaviourTreeStatus> fn)
        {
            if (parentNodeStack.Count <= 0)
            {
                throw new ApplicationException("Can't create an unnested ActionNode, it must be a leaf node.");
            }

            var actionNode = new ActionNode<TTickData>(name, fn);
            parentNodeStack.Peek().AddChild(actionNode);
            return this;
        }

        /// <summary>
        /// Like an action node... but the function can return true/false and is mapped to success/failure.
        /// </summary>
        public BehaviourTreeBuilder<TTickData> Condition(string name, Func<TTickData, bool> fn)
        {
            return Do(name, t => fn(t) ? BehaviourTreeStatus.Success : BehaviourTreeStatus.Failure);
        }

        /// <summary>
        /// Create an inverter node that inverts the success/failure of its children.
        /// </summary>
        public BehaviourTreeBuilder<TTickData> Inverter(string name)
        {
            var inverterNode = new InverterNode<TTickData>(name);

            if (parentNodeStack.Count > 0)
            {
                parentNodeStack.Peek().AddChild(inverterNode);
            }

            parentNodeStack.Push(inverterNode);
            return this;
        }

        /// <summary>
        /// Create a sequence node.
        /// </summary>
        public BehaviourTreeBuilder<TTickData> Sequence(string name)
        {
            var sequenceNode = new SequenceNode<TTickData>(name);

            if (parentNodeStack.Count > 0)
            {
                parentNodeStack.Peek().AddChild(sequenceNode);
            }

            parentNodeStack.Push(sequenceNode);
            return this;
        }

        /// <summary>
        /// Create a parallel node.
        /// </summary>
        public BehaviourTreeBuilder<TTickData> Parallel(string name, int numRequiredToFail, int numRequiredToSucceed)
        {
            var parallelNode = new ParallelNode<TTickData>(name, numRequiredToFail, numRequiredToSucceed);

            if (parentNodeStack.Count > 0)
            {
                parentNodeStack.Peek().AddChild(parallelNode);
            }

            parentNodeStack.Push(parallelNode);
            return this;
        }

        /// <summary>
        /// Create a selector node.
        /// </summary>
        public BehaviourTreeBuilder<TTickData> PrioritySelector(string name)
        {
            var selectorNode = new PrioritySelectorNode<TTickData>(name);

            if (parentNodeStack.Count > 0)
            {
                parentNodeStack.Peek().AddChild(selectorNode);
            }

            parentNodeStack.Push(selectorNode);
            return this;
        }

        /// <summary>
        /// Create a selector node.
        /// </summary>
        public BehaviourTreeBuilder<TTickData> PropabilitySelector(string name)
        {
            var selectorNode = new ProbabilitySelectorNode<TTickData>(name);

            if (parentNodeStack.Count > 0)
            {
                parentNodeStack.Peek().AddChild(selectorNode);
            }

            parentNodeStack.Push(selectorNode);
            return this;
        }

        /// <summary>
        /// Splice a sub tree into the parent tree.
        /// </summary>
        public BehaviourTreeBuilder<TTickData> Splice(IBehaviourTreeNode<TTickData> subTree)
        {
            if (subTree == null)
            {
                throw new ArgumentNullException("subTree");
            }

            if (parentNodeStack.Count <= 0)
            {
                throw new ApplicationException("Can't splice an unnested sub-tree, there must be a parent-tree.");
            }

            parentNodeStack.Peek().AddChild(subTree);
            return this;
        }

        /// <summary>
        /// Build the actual tree.
        /// </summary>
        public IBehaviourTreeNode<TTickData> Build()
        {
            if (curNode == null)
            {
                throw new ApplicationException("Can't create a behaviour tree with zero nodes");
            }
            return curNode;
        }

        /// <summary>
        /// Ends a sequence of children.
        /// </summary>
        public BehaviourTreeBuilder<TTickData> End()
        {
            curNode = parentNodeStack.Pop();
            return this;
        }
    }
}
