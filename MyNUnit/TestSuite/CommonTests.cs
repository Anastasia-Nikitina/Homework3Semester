namespace TestSuite;

using Attributes;

public class CommonTests
{
    public static int Counter = 0;
    
    [BeforeClass]
    public static void BeforeClass()
    {
        Interlocked.Increment(ref Counter);
    }

    [Before]
    public void Before()
    {
        Interlocked.Add(ref Counter, 5);
    }

    [Test]
    public void PassedTest()
    {
        Interlocked.Add(ref Counter, -5);
    }

    [Test]
    public void FailedTest()
    {
        throw new Exception();
    }

    [After]
    public void After()
    {
        Interlocked.Add(ref Counter, 10);
    }

    [AfterClass]
    public static void AfterClass()
    {
        Interlocked.Add(ref Counter, -10);
    }
}