using NUnit.Framework;
using TestSuite;

namespace NUnitTest;

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
        Assert.AreEqual(CommonTests.Counter, 55);
    }
    
    [Test]
    public void ForIgnoredAndExpectedTests()
    {
        Assert.AreEqual(IgnoredAndExpectedTests.Counter, 1);
    }
}       