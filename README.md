# Fluent-Behaviour-Tree

C# behaviour tree library with a fluent API.

For a background and walk-through please see [the accompanying article](http://www.what-could-possibly-go-wrong.com/fluent-behavior-trees-for-ai-and-game-logic/). 

## Understanding Behaviour Trees

Here are some resources to help you understand behaviour trees:

- [Behaviour tree (Wikipedia)](https://en.wikipedia.org/wiki/Behavior_tree_(artificial_intelligence,_robotics_and_control))
- [Behavior trees for AI: How they work](http://www.gamasutra.com/blogs/ChrisSimpson/20140717/221339/Behavior_trees_for_AI_How_they_work.php)
- [Understanding Behaviour Trees](http://aigamedev.com/open/article/bt-overview/)
- [Introduction and implementation of Behaviour Trees](http://guineashots.com/2014/07/25/an-introduction-to-behavior-trees-part-1/)

## Installation

In the Visual Studio [Package Manager Console](http://docs.nuget.org/consume/package-manager-console):

	PM> Install-Package FluentBehaviourTree

Or clone or download the code from [the github repository](https://github.com/codecapers/fluent-behaviour-tree).

## Creating a Behaviour Tree

A behaviour tree is created through *BehaviourTreeBuilder*. The tree is returned when the *Build* function is called.

	using FluentBehaviourTree;

	...

	IBehaviourTreeNode tree;

	public void Startup()
	{
		var builder = new BehaviourTreeBuilder();
		this.tree = builder
			.Sequence("my-sequence")
				.Do("action1",  t => 
				{
					// Action 1.
					return BehaviourTreeStatus.Success;
				})
				.Do("action2", t => 
				{
					// Action 2.
					return BehaviourTreeStatus.Success;
				})
			.End()
			.Build();
	}

*Tick* the behaviour tree on each *update* of your *game loop*:

	public void Update(float deltaTime)
	{
		this.tree.Tick(new TimeData(deltaTime));
	}

## Behaviour Tree Status

Behaviour tree nodes return the following status codes:

* *Success*: The node has finished what it was doing and succeeded.
* *Failure*: The node has finished, but failed.
* *Running*: The node is still working on something. 

## Node Types

### Action / Leaf-Node

Call the *Do* function to create an action node at the leaves of the behavior tree. 

	.Do("do-something", t => 
	{
		// ... do something ...
		// ... query the entity, query the environment then take some action ...
		return BehaviourTreeStatus.Success;
	}); 

The return value defines the status of the node. Return *Success*, *Failure* or *Running*.

### Sequence

Runs each child node in sequence. Fails for the first child node that *fails*. Moves to the next child when the current running child *succeeds*. Stays on the current child node while it returns *running*. Succeeds when all child nodes have succeeded.

	.Sequence("my-sequence")
		.Do("action1", t => 
		{
			// Sequential action 1.
			return BehaviourTreeStatus.Success; // Run this.
		}) 
		.Do("action2", t => 
		{
			// Sequential action 2.
			return BehaviourTreeStatus.Success; // Then run this.
		})
	.End()

### Parallel

Runs all child nodes in parallel. Continues to run until a required number of child nodes have either *failed* or *succeeded*.

	int numRequiredToFail = 2;
	int numRequiredToSucceed = 2;

	.Parallel("my-parallel", numRequiredToFail, numRequiredToSucceed)
		.Do("action1", t => 
		{
			// Parallel action 1.
			return BehaviourTreeStatus.Running;
		})
		.Do("action2", t => 
		{
			// Parallel action 2.
			return BehaviourTreeStatus.Running;
		})		
	.End()

### Selector

Runs child nodes in sequence until it finds one that *succeeds*. Succeeds when it finds the first child that *succeeds*. For child nodes that *fail* it moves forward to the next child node. While a child is *running* it stays on that child node without moving forward. 

	.Selector("my-selector")
		.Do("action1", t => 
		{
			// Action 1.
			return BehaviourTreeStatus.Failure; // Fail, move onto next child.
		}); 
		.Do("action2", t => 
		{
			// Action 2.
			return BehaviourTreeStatus.Success; // Success, stop here.
		})		
		.Do("action3", t => 
		{
			// Action 3.
			return BehaviourTreeStatus.Success; // Doesn't get this far. 
		})		
	.End()


### Condition

The condition function is syntactic sugar for the *Do* function. It allows return of a boolean value that is then converted to a *success* or *failure*. It is intended to be used with *Selector*.

	.Selector("my-selector")
		.Condition("condition1", t => SomeBooleanCondition())	// Predicate that returns *true* or *false*. 
		.Do("action1", t => SomeAction())					// Action to run if the predicate evaluates to *true*. 
	.End()


### Inverter

Inverts the *success* or *failure* of the child node. Continues running while the child node is *running*.

	.Inverter("inverter1")
		.Do("action1", t => BehaviourTreeStatus.Success) // *Success* will be inverted to *failure*.
	.End() 


	.Inverter("inverter1")
		.Do("action1", t => BehaviourTreeStatus.Failure) // *Failure* will be inverted to *success*.
	.End() 

## Nesting Behaviour Trees

Behaviour trees can be nested to any depth, for example:

	.Selector("parent")
		.Sequence("child-1")
			...
			.Parallel("grand-child")
				...
			.End()
			...
		.End()
		.Sequence("child-2")
			...
		.End()
	.End()

## Splicing a Sub-tree

Separately created sub-trees can be spliced into parent trees. This makes it easy to build behaviour trees from reusable components.

	private IBehaviourTreeNode CreateSubTree()
	{
		var builder = new BehaviourTreeBuilder();
		return builder
			.Sequence("my-sub-tree")
				.Do("action1", t => 
				{
					// Action 1.
					return BehaviourTreeStatus.Success;
				})
				.Do("action2", t => 
				{
					// Action 2.
					return BehaviourTreeStatus.Success;
				}); 
			.End()
			.Build();
	}

	public void Startup()
	{
		var builder = new BehaviourTreeBuilder();
		this.tree = builder
			.Sequence("my-parent-sequence")
				.Splice(CreateSubTree()) // Splice the child tree in.
				.Splice(CreateSubTree()) // Splice again.
			.End()
			.Build();
	}