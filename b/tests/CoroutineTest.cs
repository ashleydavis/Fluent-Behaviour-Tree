using FluentBehaviourTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using System.Diagnostics;
using System.Reflection;

namespace tests
{
    public class CoroutineTest
    {
        TestCoroutine tc;
        void Init()
        {
            tc = new TestCoroutine();
        }

        [Fact]
        public void testCoroutine()
        {
            Init();
            tc.Start();
            Console.Write(tc.report);
        }

       

    }
}
