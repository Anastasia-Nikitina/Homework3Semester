namespace Lazy;

using System;

/// <summary>
/// Single-threaded Lazy implementation
/// </summary>
public class SingleThreadedLazy<T>: ILazy<T>
{
    private bool _isCalculated;
    private Func<T> _supplier;
    private T _result;
    
    /// <summary>
    /// Create a new  Single-threaded Lazy object 
    /// </summary>
    public SingleThreadedLazy(Func<T> supplier)
    {
        ArgumentNullException.ThrowIfNull(supplier);
        _supplier = supplier;
    }

    /// <summary>
    /// Return result of calculation
    /// </summary>
    public T Get()
    {
        if (_isCalculated)
        {
            return _result;
        }
        _result = _supplier();
        _isCalculated = true;
        _supplier = null!;
        return _result;
    }
}