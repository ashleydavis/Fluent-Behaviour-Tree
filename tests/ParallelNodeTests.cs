﻿using FluentBehaviourTree;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace tests
{
    public class ParallelNodeTests
    {
        ParallelNode testObject;

        void Init(int numRequiredToFail = 0, int numRequiredToSucceed = 0)
        {
            testObject = new ParallelNode("some-parallel", numRequiredToFail, numRequiredToSucceed);
        }

        [Fact]
        public void runs_all_nodes_in_order()
        {
            Init();

            var time = new TimeData();

            var callOrder = 0;

            var mockChild1 = new Mock<IBehaviourTreeNode>();
            mockChild1
                .Setup(m => m.Tick(time))
                .Returns(TreeStatus.getStatus(new BehaviourTreeStatus[] { BehaviourTreeStatus.Running, BehaviourTreeStatus.Success }))
                .Callback(() =>
                {
                    Assert.Equal(1, ++callOrder);
                });

            var mockChild2 = new Mock<IBehaviourTreeNode>();
            mockChild2
                .Setup(m => m.Tick(time))
                .Returns(TreeStatus.getStatus(new BehaviourTreeStatus[] { BehaviourTreeStatus.Running, BehaviourTreeStatus.Failure }))
                .Callback(() =>
                 {
                     Assert.Equal(2, ++callOrder);
                 });

            testObject.AddChild(mockChild1.Object);
            testObject.AddChild(mockChild2.Object);
            
            var e = testObject.Tick(time);
            e.MoveNext();
            Assert.Equal(BehaviourTreeStatus.Running, e.Current);

            Assert.Equal(2, callOrder);

            mockChild1.Verify(m => m.Tick(time), Times.Once());
            mockChild2.Verify(m => m.Tick(time), Times.Once());
            Console.WriteLine("Test Object Hierarchy after Execution: ");
            Console.WriteLine(testObject.getTreeAsString(" --> "));
        }

        [Fact]
        public void fails_when_required_number_of_children_fail()
        {
            Init(2, 2);

            var time = new TimeData();

            var mockChild1 = new Mock<IBehaviourTreeNode>();
            mockChild1
                .Setup(m => m.Tick(time))
                .Returns(TreeStatus.getStatus(BehaviourTreeStatus.Failure));

            var mockChild2 = new Mock<IBehaviourTreeNode>();
            mockChild2
                .Setup(m => m.Tick(time))
                .Returns(TreeStatus.getStatus(BehaviourTreeStatus.Failure));

            var mockChild3 = new Mock<IBehaviourTreeNode>();
            mockChild3
                .Setup(m => m.Tick(time))
                .Returns(TreeStatus.getStatus( new BehaviourTreeStatus[] { BehaviourTreeStatus.Running, BehaviourTreeStatus.Success}));

            testObject.AddChild(mockChild1.Object);
            testObject.AddChild(mockChild2.Object);
            testObject.AddChild(mockChild3.Object); 
            var e = testObject.Tick(time);
            e.MoveNext();
            Assert.Equal(BehaviourTreeStatus.Failure, e.Current);

            mockChild1.Verify(m => m.Tick(time), Times.Once());
            mockChild2.Verify(m => m.Tick(time), Times.Once());
            mockChild3.Verify(m => m.Tick(time), Times.Once());
        }

        [Fact]
        public void succeeeds_when_required_number_of_children_succeed()
        {
            Init(2, 2);

            var time = new TimeData();

            var mockChild1 = new Mock<IBehaviourTreeNode>();
            mockChild1
                .Setup(m => m.Tick(time))
                .Returns(TreeStatus.getStatus(BehaviourTreeStatus.Success));

            var mockChild2 = new Mock<IBehaviourTreeNode>();
            mockChild2
                .Setup(m => m.Tick(time))
                .Returns(TreeStatus.getStatus(BehaviourTreeStatus.Success));

            var mockChild3 = new Mock<IBehaviourTreeNode>();
            mockChild3
                .Setup(m => m.Tick(time))
                .Returns(TreeStatus.getStatus(new BehaviourTreeStatus[] { BehaviourTreeStatus.Running, BehaviourTreeStatus.Failure }));

            testObject.AddChild(mockChild1.Object);
            testObject.AddChild(mockChild2.Object);
            testObject.AddChild(mockChild3.Object);
             
            var e = testObject.Tick(time);
            e.MoveNext();
            Assert.Equal(BehaviourTreeStatus.Success,e.Current);

            mockChild1.Verify(m => m.Tick(time), Times.Once());
            mockChild2.Verify(m => m.Tick(time), Times.Once());
            mockChild3.Verify(m => m.Tick(time), Times.Once());
        }

        [Fact]
        public void continues_to_run_if_required_number_children_neither_succeed_or_fail()
        {
            Init(2, 2);

            var time = new TimeData();

            var mockChild1 = new Mock<IBehaviourTreeNode>();
            mockChild1
                .Setup(m => m.Tick(time))
                .Returns(TreeStatus.getStatus(BehaviourTreeStatus.Success));

            var mockChild2 = new Mock<IBehaviourTreeNode>();
            mockChild2
                .Setup(m => m.Tick(time))
                .Returns(TreeStatus.getStatus(new BehaviourTreeStatus[] { BehaviourTreeStatus.Running, BehaviourTreeStatus.Failure }));

            testObject.AddChild(mockChild1.Object);
            testObject.AddChild(mockChild2.Object);
            
            var e = testObject.Tick(time);
            e.MoveNext();

            Assert.Equal(BehaviourTreeStatus.Running, e.Current);

            mockChild1.Verify(m => m.Tick(time), Times.Once());
            mockChild2.Verify(m => m.Tick(time), Times.Once());
        }
    }
}
