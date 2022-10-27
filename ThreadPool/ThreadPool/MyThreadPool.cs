namespace ThreadPool;
using System.Collections.Concurrent;

/// <summary>
/// Class for custom thread pool
/// </summary>
public class MyThreadPool
{
    private readonly Thread[] _threads;
    private readonly CancellationTokenSource _cts = new();
    private readonly ConcurrentQueue<Action?> _tasks = new();
    private readonly int _countOfThreads;
    private int _countOfFinishedThreads;
    private readonly AutoResetEvent _indicatorOfNewTask = new(false);
    private readonly AutoResetEvent _indicatorOfFreeThread = new(false);
    private readonly object _lockObject = new();
    
    /// <summary>
    /// constructor MyThreadPool
    /// </summary>
    public MyThreadPool(int countOfThreads)
    {
        if (countOfThreads < 1)
        {
            throw new ArgumentOutOfRangeException("Number of threads should be positive");
        }
        
        _countOfThreads = countOfThreads;
        _threads = new Thread[_countOfThreads];
        for (var i = 0; i < _countOfThreads; i++)
        {
            _threads[i] = new Thread(() =>
                {
                    while (!_cts.IsCancellationRequested || !_tasks.IsEmpty)
                    {
                        if (_tasks.TryDequeue(out var func))
                        {
                            _indicatorOfNewTask.Set();
                            func?.Invoke();
                        }
                        else
                        {
                            _indicatorOfNewTask.WaitOne();
                        }
                    }
                    Interlocked.Increment(ref _countOfFinishedThreads);
                    _indicatorOfFreeThread.Set();
                });
               _threads[i].Start();
        }
    }
    
    /// <summary>
    /// Shuts down thread
    /// </summary>
    public void Shutdown()
    {
        lock (_lockObject)
        {
            _cts.Cancel(); 
        }
        while (_countOfFinishedThreads != _threads.Length)
        {
            _indicatorOfNewTask.Set();
            _indicatorOfFreeThread.WaitOne();
        }
    }

    /// <summary>
    /// Adds task to the thread pool
    /// </summary>
    public IMyTask<T> AddTask<T>(Func<T> function)
    {
        lock (_lockObject)
        {
            if (_cts.Token.IsCancellationRequested)
            {
                throw new OperationCanceledException();
            }
            var newTask = new MyTask<T>(function, this);
            _tasks.Enqueue(newTask.Run);
            _indicatorOfNewTask.Set();
            return newTask;
        }
    }

    /// <summary>
    /// Class for custom tasks
    /// </summary>
    private class MyTask<TResult> : IMyTask<TResult>
    {
        private static Func<TResult>? _function;
        private readonly ConcurrentQueue<Action?> _nextTasks = new();
        private static TResult _result = default!;
        private Exception? _exception;
        private readonly object _lockObject = new();
        private readonly MyThreadPool _threadPool;
        private readonly ManualResetEvent _isResultCalculated = new(false);
        
        /// <summary>
        /// constructor MyTask
        /// </summary>
        public MyTask(Func<TResult> function, MyThreadPool threadPool)
        {
            _function = function;
            _threadPool = threadPool;
        }
        
        /// <summary>
        /// Returns true, if task is done
        /// </summary>
        public bool IsCompleted { get; private set; }
        
        /// <summary>
        ///  Returns result of executing task
        /// </summary>
        public TResult Result
        {
            get
            {
                _isResultCalculated.WaitOne();
                if (_exception != null)
                {
                    throw new AggregateException(_exception); 
                }
                return _result;
            }
        }
        
        /// <summary>
        /// Returns a new task, accepted for execution
        /// </summary>
        public IMyTask<TNewResult> ContinueWith<TNewResult>(Func<TResult, TNewResult> func)
        {
            if (_threadPool._cts.Token.IsCancellationRequested)
            {
                throw new OperationCanceledException();
            }

            lock (_lockObject)
            {
                if (IsCompleted)
                {
                    return _threadPool.AddTask(() => func(_result));
                }
                var nextTask = new MyTask<TNewResult>(() => func(_result), _threadPool);
                _nextTasks.Enqueue(nextTask.Run);
                
                return nextTask;
            }
        }
        
        /// <summary>
        /// Starts a thread pool
        /// </summary>
        public void Run()
        {
            try
            {
                if (_function != null)
                {
                    _result = _function();
                }
            }
            catch (Exception exception)
            {
                _exception = new AggregateException(exception);
            }

            lock (_lockObject)
            {
                _function = null;
                IsCompleted = true;
                _isResultCalculated.Set();
                while (!_nextTasks.IsEmpty)
                {
                    if (_nextTasks.TryDequeue(out Action? nextTask))
                    {
                        _threadPool._tasks.Enqueue(nextTask);
                        _threadPool._indicatorOfNewTask.Set();
                    }
                }
            }
        }
    }
}