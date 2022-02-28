namespace MultMatrices;

using System;
using static WorkWithFiles;
using static Matrix;

public static class Program
{
    private static void Main(string[] args)
    {
        if (args.Length != 2)
        {
            Console.WriteLine("You need to enter two arguments: the paths to the files with the first and second matrices.");
        }
        else
        {
            var matrix1 = ReadMatrix(args[0]);
            var matrix2 = ReadMatrix(args[1]);
            var res = ParallelMatrixMultiplication(matrix1, matrix2);
            WriteMatrix(res, args[2]);
        }
    }
}