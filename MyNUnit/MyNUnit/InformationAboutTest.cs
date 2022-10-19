namespace MyNUnit;

/// <summary>
/// Class for information about result of test
/// </summary>
public class InformationAboutTest
{
    /// <summary>
    /// The method that returns name of test
    /// </summary>
    public string Name { get; }
    
    /// <summary>
    /// The method that returns result of test
    /// </summary>
    public string Result { get; }
    
    /// <summary>
    /// The method that returns the test execution time
    /// </summary>
    public float Time { get; }
    
    /// <summary>
    /// The method that returns reason of ignore
    /// </summary>
    public string ReasonOfIgnore { get; }

    public InformationAboutTest(string name, string result,  float time, string reasonOfIgnore)
    {
        Name = name;
        Result = result;
        Time = time;
        ReasonOfIgnore = reasonOfIgnore;
    }
}