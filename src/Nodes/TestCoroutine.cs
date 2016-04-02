using System;
using System.Collections;
using System.Collections.Generic;


public enum BStatus
{
    Success,
    Failure,
    Running,
    Paused
}

public class TestCoroutine 
{
    public TestCoroutine()
    {

    }
    public string report = "";
    bool done = false;
    public void Start()
    {
        Log("Before StartCoroutine()");
        IEnumerator<BStatus> f1 = (Outer("Some Input Value", (myReturnValue, location) => {
            Log("Returned Value from " + location + " is: " + myReturnValue);
        }));
        for (var e = f1; e.MoveNext();)
        {
           Console.WriteLine(" --> Result is: "+e.Current);
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
        report +=  ": " + msg + "\n";
        //Debug.Log(Time.frameCount + ": " + msg);
    }

    IEnumerator<BStatus> Outer(string inputval, System.Action<BStatus, string> callback)
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

    IEnumerator<BStatus> Inner1()
    {
        Log("Beginning of Inner1()");
        yield return BStatus.Running;

        Log("Middle of Inner1()");

        for (var e = Inner2(); e.MoveNext();)
        {
            yield return e.Current;
        }
        yield return BStatus.Success;
        Log("End of Inner1()");
    }

    IEnumerator<BStatus> Inner2()
    {
        Log("Beginning of Inner2()");
        yield return BStatus.Running;
        Log("Middle of Inner2()");
        yield return BStatus.Success;
        Log("End of Inner2()");
    }


    IEnumerator<BStatus> Inner3()
    {
        Log("Beginning of Inner3()");
        yield return BStatus.Running;

        Log("Middle of Inner3()");

        for (var e = Inner4(); e.MoveNext();)
        {
            yield return e.Current;
        }
        yield return BStatus.Success;
        Log("End of Inner3()");
    }

    IEnumerator<BStatus> Inner4()
    {
        Log("Beginning of Inner4()");
        yield return BStatus.Running;
        Log("Middle of Inner4()");
        yield return BStatus.Success;
        Log("End of Inner4()");
    }
}
