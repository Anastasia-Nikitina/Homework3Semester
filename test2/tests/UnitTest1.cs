using System.IO;
using NUnit.Framework;
using test2;


namespace tests
{
    public class Tests
    {

        [Test]
        public void ExceptionOneThred()
            {
                Assert.Throws<FileNotFoundException>(() => CalcCheckSum.OneThreadCalculation("qwerty123"));
            }
        [Test]
        public void ExceptionMultThread()
        {
            Assert.Throws<FileNotFoundException>(() => CalcCheckSum.MultThreadCalculation("qwerty123"));
        }
        
    }
}