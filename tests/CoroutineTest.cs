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

    public class TestCoroutine
    {
        public string report = "";
        bool done = false;
        public void Start()
        {
            Log("Before StartCoroutine()");
            IEnumerator<BehaviourTreeStatus> f1 = (Outer("Some Input Value", (myReturnValue, location) =>
            {
                Log("Returned Value from " + location + " is: " + myReturnValue);
            }));
            for (var e = f1; e.MoveNext();)
            {
                Console.WriteLine(" --> Result is: " + e.Current);
            }
            Log("End Iteration of Coroutine()");
        }

        public void Update()
        {
            if (!done)
                Log("---> Called from Update");
        }

        public void Log(string msg)
        {
            report += ": " + msg + "\n";
            //Debug.Log(Time.frameCount + ": " + msg);
        }

        IEnumerator<BehaviourTreeStatus> Outer(string inputval, System.Action<BehaviourTreeStatus, string> callback)
        {
            Log("Beginning of Outer()");
            for (var e = Inner1(); e.MoveNext();)
            {
                yield return e.Current;
                callback(e.Current, "From Inner1");
            }
            Log("Middle of Outer()");
            for (var e = Inner3(); e.MoveNext();)
            {
                yield return e.Current;
                callback(e.Current, "From Inner3");
            }
            Log("End of Outer() - Done=true");
            done = true;
            Console.WriteLine(report);
        }

        IEnumerator<BehaviourTreeStatus> Inner1()
        {
            Log("Beginning of Inner1()");
            yield return BehaviourTreeStatus.Running;

            Log("Middle of Inner1()");

            for (var e = Inner2(); e.MoveNext();)
            {
                yield return e.Current;
            }
            yield return BehaviourTreeStatus.Success;
            Log("End of Inner1()");
        }

        IEnumerator<BehaviourTreeStatus> Inner2()
        {
            Log("Beginning of Inner2()");
            yield return BehaviourTreeStatus.Running;
            Log("Middle of Inner2()");
            yield return BehaviourTreeStatus.Success;
            Log("End of Inner2()");
        }


        IEnumerator<BehaviourTreeStatus> Inner3()
        {
            Log("Beginning of Inner3()");
            yield return BehaviourTreeStatus.Running;

            Log("Middle of Inner3()");

            for (var e = Inner4(); e.MoveNext();)
            {
                yield return e.Current;
            }
            yield return BehaviourTreeStatus.Success;
            Log("End of Inner3()");
        }

        IEnumerator<BehaviourTreeStatus> Inner4()
        {
            Log("Beginning of Inner4()");
            yield return BehaviourTreeStatus.Running;
            Log("Middle of Inner4()");
            yield return BehaviourTreeStatus.Success;
            Log("End of Inner4()");
        }
    }
}
