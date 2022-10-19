namespace Attributes;

/// <summary>
/// Attribute for methods, that are called before the test
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class Before : Attribute
{
}