namespace ThreadPoll.Tests;

using System;
using System.Threading;
using NUnit.Framework;
using ThreadPool;

public class Tests
{
    private MyThreadPool _threadPool;
    private int counter;

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
        Assert.AreEqual(9, _threadPool.AddTask(func).Result);
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

    [Test]
    public void TwoTasksTest()
    {
        var task1 = _threadPool.AddTask(() => Interlocked.Decrement(ref counter));
        var task2 = _threadPool.AddTask(() => "cool test".ToUpper());
        Assert.AreEqual(-1, task1.Result);
        Assert.AreEqual("COOL TEST", task2.Result);
    }

    [Test]
    public void ExceptionWhenCountOfThreadsIsNegative()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            var myThreadPool = new MyThreadPool(-34);
        });
    }

    
    [Test]
    public void TestForMultiThreading()
    {
        var manualResetEvent = new ManualResetEvent(false);
        var thread1 = new Thread(() =>
        {
            manualResetEvent.WaitOne();
            Assert.Throws<OperationCanceledException>(() => _threadPool.AddTask(() => 100 + 1));
        });
        var thread2 = new Thread(() =>
        {
            _threadPool.Shutdown();
            manualResetEvent.Set();
            
        });
        thread1.Start();
        thread2.Start();
        thread1.Join();
        thread2.Join();
        var otherThreadPool = new MyThreadPool(10);
        manualResetEvent.Reset();
        var thread3 = new Thread(() =>
        {
            var task = otherThreadPool.AddTask(() => "ok");
            manualResetEvent.Set();
            Assert.AreEqual("ok", task.Result);
            
        });
        var thread4 = new Thread(() =>
        {
            manualResetEvent.WaitOne();
            otherThreadPool.Shutdown();
            
        });
        thread3.Start();
        thread4.Start();
        thread3.Join();
        thread4.Join();
    }
}