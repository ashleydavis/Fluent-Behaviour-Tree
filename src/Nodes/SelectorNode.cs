using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentBehaviourTree
{
    /// <summary>
    /// Selects the first node that succeeds. Tries successive nodes until it finds one that doesn't fail.
    /// </summary>
    public class SelectorNode : IParentBehaviourTreeNode
    {
        /// <summary>
        /// The name of the node.
        /// </summary>
        private string name;

        /// <summary>
        /// List of child nodes.
        /// </summary>
        private List<IBehaviourTreeNode> children = new List<IBehaviourTreeNode>(); //todo: optimization, bake this to an array.

        public SelectorNode(string name)
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
            if (this.enumerator.Current == null)
                this.enumerator.MoveNext();
            while (this.enumerator.Current != null)
            {
                var childStatus = this.enumerator.Current.Tick(time);
                if (childStatus != BehaviourTreeStatus.Failure)
                {
                    if (childStatus == BehaviourTreeStatus.Success)
                        this.enumerator.Reset();
                    return childStatus;
                }

                if (!this.enumerator.MoveNext())
                    break;
            }
            this.enumerator.Reset();
            return BehaviourTreeStatus.Failure;
        }

        /// <summary>
        /// Add a child node to the selector.
        /// </summary>
        public void AddChild(IBehaviourTreeNode child)
        {
            children.Add(child);
        }
    }
}
