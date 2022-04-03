namespace Lazy;

using System;

/// <summary>
/// Multi-threaded Lazy implementation
/// </summary>
public class MultiThreadedLazy<T> : ILazy<T>
{
    private volatile bool _isCalculated;
    private Func<T> _supplier;
    private T _result;
    private readonly object _lockObject = new();
    
    /// <summary>
    /// Create a new Multi-threaded Lazy object 
    /// </summary>
    public MultiThreadedLazy(Func<T> supplier)
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
        lock (_lockObject)
        {
            if (_isCalculated)
            {
                return _result;
            }
            _result = _supplier();
            _isCalculated = true;
            _supplier = null!;
        }
        return _result;
    }
}