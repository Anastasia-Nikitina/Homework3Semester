namespace ThreadPool;

/// <summary>
/// Interface for custom task
/// </summary>

  public interface IMyTask<out TResult>
    {
      /// <summary>
      /// Returns "true", if task is completed
      /// </summary>
      public bool IsCompleted { get; }
      
      /// <summary>
      /// Returns the result of the task execution
      /// </summary>
      public TResult Result { get; }
      
      /// <summary>
      /// Adds a new task, the result of which depends of the previous one 
      /// </summary>
      public IMyTask<TNewResult> ContinueWith<TNewResult>(Func<TResult, TNewResult> newTask);

    }
    
