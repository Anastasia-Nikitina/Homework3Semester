namespace Attributes;

/// <summary>
/// Attribute for methods that are called after the test
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class After : Attribute
{
}