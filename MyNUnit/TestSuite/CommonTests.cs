using System.Diagnostics.Metrics;

namespace TestSuite;

using Attributes;

public class CommonTests
{
    private static int counter;
    [Before]
    public void Before()
    {
    }

    [Test]
    public void PassedTest()
    {
        Thread.Sleep(500);
    }

    [Test]
    public void FailedTest()
    {
        Thread.Sleep(300);
        throw new Exception();
    }

    [After]
    public void After()
    {
        Interlocked.Increment(ref counter);
    }

    [AfterClass]
    public static void AfterClass()
    {
    }
}