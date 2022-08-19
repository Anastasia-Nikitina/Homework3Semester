namespace Lazy;

using System;

/// <summary>
/// Class for creating Lazy objects
/// </summary>
public static class LazyFactory
{
    /// <summary>
    /// Creating Single-Threaded Lazy
    /// </summary>
    public static ILazy<T> CreateSingleThreadedLazy<T>(Func<T> supplier)
        => new SingleThreadedLazy<T>(supplier);
    
    /// <summary>
    /// Creating Multi-Threaded Lazy
    /// </summary>
    public static ILazy<T> CreateMultiThreadedLazy<T>(Func<T> supplier)
        => new MultiThreadedLazy<T>(supplier);
}