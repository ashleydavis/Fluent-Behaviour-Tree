# Fluent-Behaviour-Tree

C# behaviour tree library with a fluent API.

## Understanding Behaviour Trees

## Installation

In the Visual Studio [Package Manager Console](http://docs.nuget.org/consume/package-manager-console):

	PM> Install-package

Or clone or download the code from [the github repository](https://github.com/codecapers/fluent-behaviour-tree).

## Creating a Behaviour Tree

A behaviour tree is created thorough the API provided by *BehaviourTreeBuilder*. The tree is return when the *Build* function is called.

	using FluentBehaviourTree;

	...

	IBehaviourTreeNode tree;

	public void Startup()
	{
		var builder = new BehaviourTreeBuilder();
		this.tree = BehaviourTreeBuilder
			.Sequence("my-sequence")
				.Do(t => 
				{
					// Action 1.
					return BehaviourTreeStatus.Success;
				}); 
				.Do(t => 
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

Each behaviour tree node can return one of the following status codes:

- *Success*: Whatever the node was doing has finished and succeeded.
- *Failure*: Whatever the node was doing has finished, but failed.
- *Running*: Whatever the node is doing is still in progress. 

## Node Types

### Sequence

Runs each child node in sequence. Fails for the first child node that *fails*. Moves to the next child when the current running child *succeeds*. Stays on the current child node while it returns *running*. Succeeds when all child nodes have succeeded.

	.Sequence("my-sequence")
		.Do(t => 
		{
			// Sequential action 1.
			return BehaviourTreeStatus.Success; // Run this.
		}); 
		.Do(t => 
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
		.Do(t => 
		{
			// Parallel action 1.
			return BehaviourTreeStatus.Running;
		}); 
		.Do(t => 
		{
			// Parallel action 2.
			return BehaviourTreeStatus.Running;
		})		
	.End()

### Selector

Runs child nodes in sequence until it finds one that *succeeds*. Succeeds when it finds the first child that succeeds. For child nodes that *fail* it moves forward to the next child node. While a child is *running* it stays on that child node without moving forward. 

	.Selector("my-selector")
		.Do(t => 
		{
			// Action 1.
			return BehaviourTreeStatus.Failure; // Fail, move onto next child.
		}); 
		.Do(t => 
		{
			// Action 2.
			return BehaviourTreeStatus.Success; // Success, stop here.
		})		
		.Do(t => 
		{
			// Action 3.
			return BehaviourTreeStatus.Success; // Doesn't get this far. 
		})		
	.End()

### Inverter

Inverts the *success* or *failure* of the child node. Continues running while the child node is *running*.

	.Inverter()
		.Do(t => BehaviourTreeStatus.Success) // *Success* will be inverted to *failure*.
	.End() 


	.Inverter()
		.Do(t => BehaviourTreeStatus.Failure) // *Failure* will be inverted to *success*.
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
		return BehaviourTreeBuilder
			.Sequence("my-sub-tree")
				.Do(t => 
				{
					// Action 1.
					return BehaviourTreeStatus.Success;
				}); 
				.Do(t => 
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
		this.tree = BehaviourTreeBuilder
			.Sequence("my-parent-sequence")
				.Splice(CreateSubTree()) // Splice the child tree in.
				.Splice(CreateSubTree()) // Splice again.
			.End()
			.Build();
	}