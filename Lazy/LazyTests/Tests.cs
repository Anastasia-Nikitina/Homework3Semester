namespace LazyTests;

using System;
using System.Threading;
using Lazy;
using NUnit.Framework;


public class Tests
{
    [Test]
    public void TestForNullSupplier()
    {
        Assert.Throws<ArgumentNullException>(() => LazyFactory.CreateSingleThreadedLazy<int>(null));
        Assert.Throws<ArgumentNullException>(() => LazyFactory.CreateMultiThreadedLazy<int>(null));
    }

    [Test]
    public void TestForRepeatCalls()
    {
        var a = 1;
        var function = LazyFactory.CreateSingleThreadedLazy(() => 1);
        while (a != 10)
        {
            Assert.AreEqual(1, function.Get());
            a++;
        }
    }

    [Test]
    public void TestForRaces()
    {
        var k = 0;
        var result = 0;
        var function = LazyFactory.CreateMultiThreadedLazy(() =>  Interlocked.Increment(ref k));
        var threads = new Thread[1000];

        for (var i = 0; i < threads.Length; ++i)
        {
            threads[i] = new Thread(() => Interlocked.Add(ref result, function.Get()));
        }

        foreach (var thread in threads)
        {
            thread.Start();
        }
        
        foreach (var thread in threads)
        {
            thread.Join();
        }
        Assert.AreEqual(1000, result);
    }
}