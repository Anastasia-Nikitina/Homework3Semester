using NUnit.Framework;
using System;
using static MultMatrices.Matrix;
using static MultMatrices.WorkWithFiles;

namespace MultMatricesTest
{
    public class Tests
    {
        [TestCase(10, 20, 30)]
        [TestCase(100, 100, 100)]
        [TestCase(500, 300, 720)]
        public void ComparisionParAndSeqMult(int rows1, int columns1AndRows2, int columns2)
        {
            var matrix1 = GenerateMatrix(rows1, columns1AndRows2);
            var matrix2  = GenerateMatrix (columns1AndRows2, columns2);
            var seqRes = SeqMultMatrix(matrix1, matrix2);
            var parRes = ParMultMatrix(matrix1, matrix2);
            Assert.IsTrue (IsMatricesEqual(seqRes, parRes));
        }

        [TestCase(10, 20, 30, 40)]
        [TestCase(99, 100, 80, 3)]
        [TestCase(700, 550, 300, 1000)]
        public void TestForExceptions(int rows1, int columns1, int rows2, int columns2)
        {
            var matrix1 = GenerateMatrix(rows1, columns1);
            var matrix2  = GenerateMatrix (rows2, columns2);
            Assert.Throws<ArgumentException>(() => SeqMultMatrix(matrix1, matrix2));
            Assert.Throws<ArgumentException>(() => ParMultMatrix(matrix1, matrix2));
        }
        [Test]
        public void TestsForFileFunctions()
        {
            var matrix1 = GenerateMatrix(10, 10);
            var path = System.IO.Path.GetTempPath() + Guid.NewGuid() + ".txt";
            WriteMatrix(matrix1, path);
            var matrix2 = ReadMatrix(path);
            Assert.IsTrue(IsMatricesEqual(matrix1, matrix2));
        }
    }
}