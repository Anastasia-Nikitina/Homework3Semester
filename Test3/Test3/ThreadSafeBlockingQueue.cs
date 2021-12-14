using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Test3
{
    /// <summary>
    /// Class implementing thread-safe blocking queue
    /// </summary>
    public class ThreadSafeBlockingQueue<T>
    {
        private readonly object _lockObject = new ();
        private readonly SortedList<int, Queue<T>> _queue = new ();
        
        /// <summary>
        /// Method that add value with given priority
        /// </summary>
        public void Enqueue(T value, int priority)
        {
            lock (_lockObject)
            {
                if (_queue.ContainsKey(priority))
                {
                    _queue[priority].Enqueue(value);

                }
                else
                {
                    var addQueue = new Queue<T>();
                    addQueue.Enqueue(value);
                    _queue.Add(priority, addQueue);
                }
                Monitor.PulseAll(_lockObject);
            }
            
        }
        /// <summary>
        /// Method that returns value with max priority and removes this value
        /// </summary>
        public T Dequeue()
        {
            T valueWithMaxPriority = default;
            lock (_lockObject)
            {
                if (_queue.Count != 0)
                {
                    valueWithMaxPriority = _queue.Last().Value.Dequeue();
                    if (_queue.Last().Value.Count == 0)
                    {
                        _queue.Remove(_queue.Last().Key);
                    }
                }
                else
                {
                    Monitor.Wait(_lockObject);
                }
            }
            return valueWithMaxPriority;
        }
        /// <summary>
        /// Method that calculate size of queue
        /// </summary>
        public int Size()
        {
            int size;
            lock (_lockObject)
            {
                size = _queue.Count;
            }
            return size;
        }
    }
}