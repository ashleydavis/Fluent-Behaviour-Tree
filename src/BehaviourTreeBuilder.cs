using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*
     Fixes Implemented in the git hub version of the code:

            Classes Changed: BehaviourTreeBuilder.cs
            ========================================
          - in BehaviourTreeBuilder.End(), Fix to not pop the parentNodeStack if it only has one item. This ensures parentNodeStack always has at least the first container
            definition and when the tree is being built will contain other parent containers stacked on top. The number of stacked containers will depend on how often End() is called
            at the end of each Parent Container Definition.
          - Fix .Build() to return the last item in the stack. This was returning first item which will skip all other containers nested in the first parent in the tree. 
            Ticking the first container item in the stack will only execute nodes defined in the last parent container of the tree.

            Classes Changed: - ActionNode.cs, SelectorNode.cs
            =================================================
          - Fix Condition Node which is modeled as an Action Node and not as a  container. If a Selector is the parent container of the Condition, 
             then the selector container will only execute  and return the Condition node result. This is because the behavior of the selector is to return the results of the
             first successful leaf node. Any other child nodes that follow after the condition never gets executed. This was fixed by adding an indicator to the
             condition so that the action can be identified as a condition when executing in the selector. If condition returns success, the selctor will now execute
             and return when the first child (skipping the condition) is successful.

     Documentation Notes for Behaviour Tree System: 

          - There are only 2 major types of nodes:  Parent(Container) and Leaf(Action) nodes.
          - There are 4 kinds of Parent Nodes: Inverter, Parallel, Selector and Sequence - These are used as Parent Containers for
            Action Nodes that are defined as children in a child list. Each of these 4 containers are responsible for running all leaf nodes that
            are in their children list according to the rules and behaviour of each parent container type.  The child list of a container may contain
            the next parent container in the tree as the last item in the list.  The tree is constructed at build time by using a parentNodeStack which
            will accumulate the nodes as the tree is defined using the behaviourTreeBuilder method calls.
            When a container definition is encountered (.Selector(), .Sequence() .. etc) - the builder does the following:
                a. Instantiates the specified container 
                b. Adds it into the child list of the current container in the parentNodeStack (by using a stack.peek().addChild(..)) - peek gets the first item in the stack
                   without removing it.
                c. Pushes the new container instance into the parentNodeStack (to represent the current container being processed)
                d. On the next action definition, it adds the new action instance into the current container instance using parentNodeStack.peek().addChild(..)
                e. If End() is called, it will pop the current container off the stack and use the previous container to hold any new child/container definitions.
                f. If End() is not called and the next item added is another container, it will add the new container instance to the child list of the current container.
                g. After Build is called, it will pop all containers off the parentNodeStack and return only the last one which contains all recursively linked
                   containers in it's child list. Calling the Tick function will walk the entire tree starting from the first action node.

            Inverter - A container that runs a child node and inverts the tick result of the child from success to failure or vice versa.
            Parallel - A container that runs all of the child nodes one after the other and returns success or failure depending on the success/failure criteria values.
            Selector - A container that runs each child action node and returns immediately when the first child is successful.
            Sequence - A container that runs each child action node in sequence and returns when all of them succeeds. If any fails, it returns the failure immediately
                       and stops processing the remaining child nodes.

          - There is one Leaf Node: ActionNode - which has been changed to have two behaviours - Condition or Not Condition. A leaf node executes an action/function

          Caveats:
          - Do not define a tree with the root being a selector as it will be the last container in the stack and will run only the first leaf/Action node that returns success
            Any other container or action nodes defined after the first successful child Action node in this container will be ignored
          - Ending a Parent Node with End() will pop the current parent off the stack (if there is more than one) and use
            the previous parent as a container for the next parent that is defined in the tree
          - Be careful in defining which type of container will host another container as a child reference because the embedded child container may not be executed depending
            on the host container behavior... this seems to apply at least only for Selectors.

     */
namespace FluentBehaviourTree
{
    /// <summary>
    /// Fluent API for building a behaviour tree.
    /// </summary>
    public class BehaviourTreeBuilder
    {
        /// <summary>
        /// Last node created.
        /// </summary>
        private IBehaviourTreeNode curNode = null;
        private Dictionary<string, IBehaviourTreeNode> nodeMap = new Dictionary<string, IBehaviourTreeNode>();

        /// <summary>
        /// Stack node nodes that we are build via the fluent API.
        /// </summary>
        private Stack<IParentBehaviourTreeNode> parentNodeStack = new Stack<IParentBehaviourTreeNode>();

        /// <summary>
        /// Create an action node.
        /// </summary>
        public BehaviourTreeBuilder Do(string name, Func<TimeData, IEnumerator<BehaviourTreeStatus>> fn)
        {
            if (parentNodeStack.Count <= 0)
            {
                throw new ApplicationException("Can't create an unnested ActionNode, it must be a leaf node.");
            }

            var actionNode = new ActionNode(name, fn);
            parentNodeStack.Peek().AddChild(actionNode);
            nodeMap.Add(name, actionNode);
            return this;
        }
       
        /// <summary>
        /// Like an action node... but the function can return true/false and is mapped to success/failure.
        /// </summary>
        public BehaviourTreeBuilder Condition(string name, Func<TimeData, bool> fn)
        {
            if (parentNodeStack.Count <= 0)
            {
                throw new ApplicationException("Can't create an unnested ActionNode, it must be a leaf node.");
            }

            var conditionNode = new ConditionNode(name, fn);
            parentNodeStack.Peek().AddChild(conditionNode);
            nodeMap.Add(name, conditionNode);
            return this;
        }

        /// <summary>
        /// Create an inverter node that inverts the success/failure of its children.
        /// </summary>
        public BehaviourTreeBuilder Inverter(string name)
        {
            //pruneStack();
            var inverterNode = new InverterNode(name);

            if (parentNodeStack.Count > 0)
            {
                parentNodeStack.Peek().AddChild(inverterNode);
            }
            
            parentNodeStack.Push(inverterNode);
            nodeMap.Add(name, inverterNode);
            return this;
        }

        /// <summary>
        /// Create a sequence node.
        /// </summary>
        public BehaviourTreeBuilder Sequence(string name)
        {
            //pruneStack();
            var sequenceNode = new SequenceNode(name);
            if (parentNodeStack.Count > 0)
            {
                parentNodeStack.Peek().AddChild(sequenceNode);
            }
           
            parentNodeStack.Push(sequenceNode);
            nodeMap.Add(name, sequenceNode);
            return this;
        }

        /// <summary>
        /// Create a parallel node.
        /// </summary>
        public BehaviourTreeBuilder Parallel(string name, int numRequiredToFail, int numRequiredToSucceed)
        {
            //pruneStack();
            var parallelNode = new ParallelNode(name, numRequiredToFail, numRequiredToSucceed);

            if (parentNodeStack.Count > 0)
            {
                parentNodeStack.Peek().AddChild(parallelNode);
            }
           
            parentNodeStack.Push(parallelNode);
            nodeMap.Add(name, parallelNode);
            return this;
        }

        /// <summary>
        /// Create a selector node.
        /// </summary>
        public BehaviourTreeBuilder Selector(string name)
        {
            //pruneStack();
            var selectorNode = new SelectorNode(name);

            if (parentNodeStack.Count > 0)
            {
                parentNodeStack.Peek().AddChild(selectorNode);
            }
           
            parentNodeStack.Push(selectorNode);
            nodeMap.Add(name, selectorNode);
            return this;
        }
        /// <summary>
        /// Create a Randomized Selector node.
        /// </summary>
        public BehaviourTreeBuilder RandomSelector(string name)
        {
            //pruneStack();
            var selectorNode = new SelectorNode(name,true);

            if (parentNodeStack.Count > 0)
            {
                parentNodeStack.Peek().AddChild(selectorNode);
            }

            parentNodeStack.Push(selectorNode);
            nodeMap.Add(name, selectorNode);
            return this;
        }
        /// <summary>
        /// Splice a sub tree into the parent tree.
        /// </summary>
        public BehaviourTreeBuilder Splice(IBehaviourTreeNode subTree)
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
            // Merge the nodeMaps and set subtree nodemap to null
            nodeMap = nodeMap.Union(subTree.nodeMap).ToDictionary(k => k.Key, v => v.Value);
            subTree.nodeMap = null;
            return this;
        }

        /// <summary>
        /// Build the actual tree.
        /// </summary>
        public IBehaviourTreeNode Build()
        {
            if (parentNodeStack.Count <= 0)
            {
                throw new ApplicationException("Can't create a behaviour tree with zero nodes");
            }
           
           if (curNode == null)
            {
                throw new ApplicationException("Can't create an unbalanced tree");
            }

            while (parentNodeStack.Count > 0)
            {
                curNode = parentNodeStack.Pop();
                // process value
            }
            curNode.nodeMap = nodeMap;
            return curNode;
        }

        /// <summary>
        /// Ends a sequence of children.
        /// </summary>
        public BehaviourTreeBuilder End()
        {
            pruneStack();
            return this;
        }
        private void pruneStack()
        {
            if (parentNodeStack.Count > 1)
                curNode = parentNodeStack.Pop();
            else
            if (parentNodeStack.Count == 1)
                curNode = parentNodeStack.Peek();

        }
    }
}
