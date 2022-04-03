namespace Lazy;

/// <summary>
/// Interface of Lazy calculations
/// </summary>
public interface ILazy<out T>
{ 
    /// <summary>
    /// Return result of calculation
    /// </summary>
    public T Get();
}