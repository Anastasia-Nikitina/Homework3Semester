namespace MyNUnit;

/// <summary>
/// Class for information about result of test
/// </summary>
public class InformationAboutTest
{
    /// <summary>
    /// Name of test
    /// </summary>
    public string Name { get; }
    
    /// <summary>
    /// Result of test
    /// </summary>
    public string Result { get; set; }

    /// <summary>
    /// Test execution time
    /// </summary>
    public float Time { get; }
    
    /// <summary>
    /// Reason of ignore
    /// </summary>
    public string ReasonOfIgnore { get; }

    public InformationAboutTest(string name, string result, float time, string reasonOfIgnore)
    {
        Name = name;
        Result = result;
        Time = time;
        ReasonOfIgnore = reasonOfIgnore;
    }
}