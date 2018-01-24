using System;
namespace FluentBehaviourTree
{
    public abstract class ParentBehaviourTreeNode<T> : IParentBehaviourTreeNode<T>
    {
        public  void AddChildren(params IBehaviourTreeNode<T>[] behaviours)
        {
            foreach (var behaviour in behaviours)
            {
                AddChild(behaviour);
            }
        }

        public abstract void AddChild(IBehaviourTreeNode<T> child);

        public abstract BehaviourTreeStatus Tick(T time);
    }
}
