namespace Attributes;

/// <summary>
/// Attribute for methods that are called before the test class
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class BeforeClass : Attribute
{
}