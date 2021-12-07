using System.IO;
using NUnit.Framework;
using test2;


namespace tests
{
    public class Tests
    {
        [Test]
        public void ExceptionOneThread()
        {
            Assert.Throws<FileNotFoundException>(() => CalcCheckSum.OneThreadCalculation("qwerty123"));
        }
        [Test]
        public void ExceptionMultThread()
        {
            Assert.ThrowsAsync<FileNotFoundException>(async () => await CalcCheckSum.MultThreadCalculation("qwerty123"));
        }
        
    }
}