using System;
using Test3;
using NUnit.Framework;

namespace Tests
{
  
    public class ThreadSafeBlockingQueueTest
    {


        [Test]
        public void ComparisonEnqueueAndDequeue()
        {
            var queue = new ThreadSafeBlockingQueue<string>();
            queue.Enqueue("qwerty", 1);
            Assert.AreEqual("qwerty", queue.Dequeue());

        }

        [Test]
        public void TestOfSize()
        {
            var queue = new ThreadSafeBlockingQueue<string>();
            Assert.AreEqual(queue.Size(), 0);
            queue.Enqueue("qwerty", 1);
            queue.Enqueue("qwerty1", 2);
            Assert.AreEqual(queue.Size(), 2);
        }
        
        
    }

}