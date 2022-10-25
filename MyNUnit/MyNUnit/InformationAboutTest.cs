namespace MyNUnit;

/// <summary>
/// Class for information about result of test
/// </summary>
public class InformationAboutTest
{
    /// <summary>
    /// Returns name of test
    /// </summary>
    public string Name { get; }
    
    /// <summary>
    /// Returns result of test
    /// </summary>
    public string Result { get; set; }

    /// <summary>
    /// Returns the test execution time
    /// </summary>
    public float Time { get; }
    
    /// <summary>
    /// Returns reason of ignore
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