namespace MyNUnit;

/// <summary>
/// Class for exceptions when calling incorrect methods
/// </summary>
public class IncorrectMethodException : Exception
{
    public IncorrectMethodException(string message)
        : base(message) { }
}