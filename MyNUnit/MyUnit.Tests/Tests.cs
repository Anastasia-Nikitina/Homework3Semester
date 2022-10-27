using System.Collections.Concurrent;
using System.Collections.Generic;
using MyNUnit;

namespace NUnitTest;

using NUnit.Framework;
using TestSuite;

public class Tests
{
    private static readonly MyNUnit.MyNUnit _myNUnit = new();
    
    private List<InformationAboutTest> _info = _myNUnit.Start("../../../../TestSuite/obj/Debug/net6.0");

    private InformationAboutTest ReturnTestInfo(List<InformationAboutTest> c, string name)
    {
        InformationAboutTest res = new("", "", 0, "");
        foreach (var x in c)
        {
            if (x.Name == name)
            {
                res = x;
            }
        }

        return res;
    }
    [Test]
    public void ForPassedTests()
    {
        var testInfo = ReturnTestInfo(_info, "PassedTest");
        Assert.AreEqual(testInfo.Result, "Passed");
        Assert.AreEqual(testInfo.ReasonOfIgnore, "");
        Assert.True(testInfo.Time >= 500);
    }
    
    [Test]
    public void ForFailedTests()
    {
        var testInfo = ReturnTestInfo(_info, "FailedTest");
        Assert.AreEqual(testInfo.Result, "Failed: occured exception: System.Exception");
        Assert.AreEqual(testInfo.ReasonOfIgnore, "");
        Assert.True(testInfo.Time >= 300);
    }
    
    [Test]
    public void ForIgnoredAndExpectedTests()
    {
        Assert.AreEqual(1, IgnoredAndExpectedTests.Counter);
    }
}       