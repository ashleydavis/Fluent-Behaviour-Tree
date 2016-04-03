using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentBehaviourTree
{
    /// <summary>
    /// Interface for behaviour tree nodes.
    /// </summary>
    public interface IBehaviourTreeNode
    {
        /// <summary>
        /// Update the time of the behaviour tree.
        /// </summary>
        IEnumerator<BehaviourTreeStatus> Tick(TimeData time);

        /// <summary>
        /// Node Parent - Present only for nodes that are hosted within a container (parent) node.
        /// </summary>
        IBehaviourTreeNode parent { get; set; }

        /// <summary>
        /// status of the current node. Initialized to initial in the constructor
        /// ActionNodes should only return 3 values: running, success or failure
        /// </summary>
        BehaviourTreeStatus currentStatus { get; set; }

        /// <summary>
        /// Sets the status of the current node as well as all child nodes (if it is a container) under it
        /// It is used to reset a behaviour tree to the initial state - in case you want to tick the
        /// tree again after it has completed without having to rebuild it from scratch
        /// </summary>
        /// <param name="aStatus"></param>
        void SetStatusAll(BehaviourTreeStatus aStatus);
        /// <summary>
        /// Returns true if parent is not null - i.e. for those nodes that are hosted within a container (parent) node.
        /// </summary>
        /// <returns></returns>
        bool hasParent();

        /// <summary>
        /// Count of all nodes that have  current status equal to the specified value in the parameter
        /// </summary>
        /// <param name="aStatus"></param>
        /// <returns></returns>
        int CountAllForStatus(BehaviourTreeStatus aStatus);
        /// <summary>
        /// nodeMap contains a dictionary map of all Nodes that have been instantiated in the tree.
        /// </summary>
        Dictionary<string, IBehaviourTreeNode> nodeMap { get; set; }
        /// <summary>
        /// Retrieves NodeMap with a recursive call all the way to the top level parent
        /// </summary>
        /// <returns></returns>
        Dictionary<string, IBehaviourTreeNode> getNodeMap();
        /// <summary>
        /// Get a specific node from the nodeMap tree dictionary
        /// </summary>
        /// <returns></returns>
        IBehaviourTreeNode getNode(string aNodeName);
        /// <summary>
        /// Gets the behavior Tree Hierarchy as a string
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        string getTreeAsString(string prefix);
    }
}
