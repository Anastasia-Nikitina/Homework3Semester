namespace MyNUnit;

using System.Reflection;

public class TypesOfMethods
{
    public List<MethodInfo> Test { get; } = new();
    public List<MethodInfo> Before { get; } = new();
    public List<MethodInfo> After { get; } = new();
    public List<MethodInfo> BeforeClass { get; } = new();
    public List<MethodInfo> AfterClass { get; } = new();
}