namespace TestSuite;

using Attributes;

public class CommonTests
{
    public static int Counter = 0;
    [BeforeClass]
    public static void BeforeClass()
    {
        Counter += 1;
    }

    [Before]
    public void Before()
    {
        Counter *= 5;
    }

    [Test]
    public void PassedTest()
    {
        Counter /= 5;
    }

    [Test]
    public void FailedTest()
    {
        throw new Exception();
    }

    [After]
    public void After()
    {
        Counter += 10;
    }

    [AfterClass]
    public static void AfterClass()
    {
        Counter -= 10;
    }
    
}