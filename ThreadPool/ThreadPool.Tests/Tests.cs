using System;

namespace ThreadPool.Tests;

using NUnit.Framework;
using ThreadPool;

public class Tests
{
    private MyThreadPool _threadPool;
    
    [SetUp]
    public void Setup()
    {
        _threadPool = new MyThreadPool(5);
    }

    [TearDown]
    public void TearDown()
    {
        _threadPool.Shutdown();
    }

    [Test]
    public void AddTaskTest()
    {
        var func = new Func<int>(() => 3 * 3);
        Assert.AreEqual(9,_threadPool.AddTask(func).Result);
    }
    
    [Test]
    public void ContinueWithTest()
    {
        var myTask = _threadPool.AddTask(() => 3 * 3).ContinueWith(x => x - 9);
        Assert.AreEqual(0, myTask.Result);
    }
    [Test]
    public void ExceptionAfterShutDownTest()
    {
        var task = _threadPool.AddTask(() => (1 + 2) * 3 / 4);
        _threadPool.Shutdown();
        Assert.Throws<OperationCanceledException>(() => task.ContinueWith(x => x.CompareTo(0)));
    }
    
    [Test]
    public void ResultAfterShutDownTest()
    {
        var task = _threadPool.AddTask(() => "Hello, world!");
        _threadPool.Shutdown();
        Assert.AreEqual("Hello, world!", task.Result);
    }
}