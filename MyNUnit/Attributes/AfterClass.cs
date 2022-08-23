namespace Attributes;

/// <summary>
/// Attribute for methods, that are called after the test class
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class AfterClass : Attribute
{
    
}