namespace MultMatricesTest;

using NUnit.Framework;
using System;
using static MultMatrices.Matrix;
using static MultMatrices.WorkWithFiles;

public class Tests
{
    [TestCase(10, 20, 30)]
    [TestCase(100, 100, 100)]
    [TestCase(500, 300, 720)]
    public void ComparisonParallelAndSequentialMultiplication(int rows1, int columns1AndRows2, int columns2)
    {
        var matrix1 = GenerateMatrix(rows1, columns1AndRows2);
        Console.WriteLine(matrix1.Array.GetLength(0));
        Console.WriteLine(matrix1.Array.GetLength(1));
        var matrix2 = GenerateMatrix(columns1AndRows2, columns2);
        Console.WriteLine(matrix2.Array.GetLength(0));
        Console.WriteLine(matrix2.Array.GetLength(1));
        var seqRes = SequentialMatrixMultiplication(matrix1, matrix2);
        var parRes = ParallelMatrixMultiplication(matrix1, matrix2);
        Assert.IsTrue(AreMatricesEqual(seqRes, parRes));
    }

    [TestCase(10, 20, 30, 40)]
    [TestCase(99, 100, 80, 3)]
    [TestCase(700, 550, 300, 1000)]
    public void TestForExceptions(int rows1, int columns1, int rows2, int columns2)
    {
        var matrix1 = GenerateMatrix(rows1, columns1);
        var matrix2 = GenerateMatrix(rows2, columns2);
        Assert.Throws<ArgumentException>(() => SequentialMatrixMultiplication(matrix1, matrix2));
        Assert.Throws<ArgumentException>(() => ParallelMatrixMultiplication(matrix1, matrix2));
    }

    [Test]
    public void TestsForFileFunctions()
    {
        var matrix1 = GenerateMatrix(10, 10);
        var path = System.IO.Path.GetTempPath() + Guid.NewGuid() + ".txt";
        WriteMatrix(matrix1, path);
        var matrix2 = ReadMatrix(path);
        Assert.IsTrue(AreMatricesEqual(matrix1, matrix2));
    }
}