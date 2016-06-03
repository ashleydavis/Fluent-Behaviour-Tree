namespace FluentBehaviourTree
{
    using System;
    using System.Collections.Generic;

    public class ProbabilitySelectorNode : ProbabilitySelectorNode<TimeData>
    {
        public ProbabilitySelectorNode(string name) : base(name)
        {
        }
    }

    public class ProbabilitySelectorNode<TTickData> : IParentBehaviourTreeNode<TTickData>
    {
        private readonly static Random rng = new Random();

        /// <summary>
        /// The name of the node.
        /// </summary>
        private readonly string name;

        /// <summary>
        /// List of child nodes.
        /// </summary>
        private readonly List<IBehaviourTreeNode<TTickData>> children = new List<IBehaviourTreeNode<TTickData>>(); //todo: optimization, bake this to an array.

        public ProbabilitySelectorNode(string name)
        {
            this.name = name;
        }

        public void AddChild(IBehaviourTreeNode<TTickData> child)
        {
            children.Add(child);
        }

        public BehaviourTreeStatus Tick(TTickData time)
        {
            var selected = children[rng.Next(children.Count)];
            return selected.Tick(time);
        }
    }
}
