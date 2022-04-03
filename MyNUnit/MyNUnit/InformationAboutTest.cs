namespace MyNUnit;

public class InformationAboutTest
{
    public string Name { get; }
    public string Result { get; }
    public float Time { get; }
    public string ReasonOfIgnore { get; }

    public InformationAboutTest(string name, string result,  float time, string reasonOfIgnore)
    {
        Name = name;
        Result = result;
        Time = time;
        ReasonOfIgnore = reasonOfIgnore;
    }
}