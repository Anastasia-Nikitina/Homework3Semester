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
        private object lockObject = new object();
        public SortedList<int, Queue<T>> queue = new SortedList<int, Queue<T>>();
        
        /// <summary>
        /// Method adding value with given priority
        /// </summary>
        public void Enqueue(T value, int priority)
        {
            lock (lockObject)
            {
                if (queue.ContainsKey(priority))
                {
                    queue[priority].Enqueue(value);

                }
                else
                {
                    var addQueue = new Queue<T>();
                    addQueue.Enqueue(value);
                    queue.Add(priority, addQueue);
                }

                Monitor.PulseAll(lockObject);
            }
            
        }
        /// <summary>
        /// Method that returns value with the greatest priority and removes it
        /// </summary>
        /// <returns></returns>
        public T Dequeue()
        {
            T valueWithMaxPriority = default;
            lock (lockObject)
            {
                if (queue.Count != 0)
                {
                    valueWithMaxPriority = queue.Last().Value.Dequeue();
                    if (queue.Last().Value.Count == 0)
                    {
                        queue.Remove(queue.Last().Key);
                    }
                }
                else
                {
                    Monitor.Wait(lockObject);
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
            lock (lockObject)
            {
                size = queue.Count;
            }
            return size;

        }
    }
}