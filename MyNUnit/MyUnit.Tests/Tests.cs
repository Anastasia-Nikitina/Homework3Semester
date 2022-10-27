namespace NUnitTest;

using NUnit.Framework;
using TestSuite;

public class Tests
{
    private readonly MyNUnit.MyNUnit _myNUnit = new();

    [SetUp]
    public void Setup()
    {
        _myNUnit.Start("../../../../TestSuite/obj/Debug/net6.0");
    }

    [Test]
    public void ForFailedAndPassedTests()
    {
        Assert.AreEqual(11, CommonTests.Counter);
    }
    
    [Test]
    public void ForIgnoredAndExpectedTests()
    {
        Assert.AreEqual(1, IgnoredAndExpectedTests.Counter);
    }
}       