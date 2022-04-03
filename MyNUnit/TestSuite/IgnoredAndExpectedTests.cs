using System.Diagnostics.Metrics;
using Attributes;


namespace TestSuite;

public class IgnoredAndExpectedTests
{
    public static int Counter = 1;

    [Test(Ignore = "This test is not needed")]
    public void IgnoredTest()
    {
        Counter -= 1;

    }
    
   [Test(Expected = typeof(ArgumentException))]
    public void ExpectedTest()
    {
        throw new ArgumentException();
    }
}