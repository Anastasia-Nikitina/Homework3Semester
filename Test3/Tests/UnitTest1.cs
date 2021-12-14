using Test3;
using NUnit.Framework;

namespace Tests
{
  
    public class ThreadSafeBlockingQueueTest
    {
        
        [Test]
        public void DequeueTest()
        {
            var queueTest = new ThreadSafeBlockingQueue<string>();
            queueTest.Enqueue("Hello", 1);
            queueTest.Enqueue("World", 2);
            var res = queueTest.Dequeue();
            Assert.AreEqual("World", res);

        }
        
        [Test]
        public void ComparisonEnqueueAndDequeue()
        {
            var queueTest = new ThreadSafeBlockingQueue<string>();
            queueTest.Enqueue("qwerty", 1);
            Assert.AreEqual("qwerty", queueTest.Dequeue());

        }

        [Test]
        public void TestOfSize()
        {
            var queueTest = new ThreadSafeBlockingQueue<string>();
            Assert.AreEqual(queueTest.Size(), 0);
            queueTest.Enqueue("qwerty", 1);
            queueTest.Enqueue("qwerty1", 2);
            Assert.AreEqual(queueTest.Size(), 2);
        }
        
    }

}