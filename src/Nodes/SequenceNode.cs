using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentBehaviourTree
{
    /// <summary>
    /// Runs child nodes in sequence, until one fails.
    /// </summary>
    public class SequenceNode : IParentBehaviourTreeNode
    {
        /// <summary>
        /// Name of the node.
        /// </summary>
        private string name;

        /// <summary>
        /// List of child nodes.
        /// </summary>
        private List<IBehaviourTreeNode> children = new List<IBehaviourTreeNode>(); //todo: this could be optimized as a baked array.

        public SequenceNode(string name)
        {
            this.name = name;
        }

        private IEnumerator<IBehaviourTreeNode> enumerator;

        public void Init()
        {
            this.enumerator = this.children.GetEnumerator();
        }

        public BehaviourTreeStatus Tick(TimeData time)
        {
            if (this.enumerator == null)
                this.Init();
            while (this.enumerator.MoveNext())
            {
                var childStatus = this.enumerator.Current.Tick(time);
                if (childStatus != BehaviourTreeStatus.Success)
                {
                    if (childStatus == BehaviourTreeStatus.Failure)
                        this.enumerator.Reset();
                    return childStatus;
                }
            } 
            this.enumerator.Reset();
            return BehaviourTreeStatus.Success;
        }

        /// <summary>
        /// Add a child to the sequence.
        /// </summary>
        public void AddChild(IBehaviourTreeNode child)
        {
            children.Add(child);
        }
    }
}
