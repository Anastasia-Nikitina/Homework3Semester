namespace MultMatrices;

using System;
using System.Threading;

public class Matrix
{
    public int[,] Array { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Matrix"/> class.
    /// Constructor of matrices.
    /// </summary>
    public Matrix(int[,] matrix)
    {
        Array = matrix;
    }

    /// <summary>
    /// Method for sequential matrix multiplication.
    /// </summary>
    /// <returns>Matrix.</returns>
    public static Matrix SequentialMatrixMultiplication(Matrix matrix1, Matrix matrix2)
    {
        if (matrix1.Array.GetLength(1) != matrix2.Array.GetLength(0))
        {
            throw new ArgumentException("Incorrect matrix sizes:(");
        }

        var result = new int[matrix1.Array.GetLength(0), matrix2.Array.GetLength(1)];
        for (var i = 0; i < matrix1.Array.GetLength(0); i++)
        {
            for (var j = 0; j < matrix2.Array.GetLength(1); j++)
            {
                for (var k = 0; k < matrix1.Array.GetLength(1); k++)
                {
                    result[i, j] += matrix1.Array[i, k] * matrix2.Array[k, j];
                }
            }
        }

        return new Matrix(result);
    }

    /// <summary>
    /// Method for parallel matrix multiplication.
    /// </summary>
    /// <returns>Matrix.</returns>
    public static Matrix ParallelMatrixMultiplication(Matrix matrix1, Matrix matrix2)
    {
        if (matrix1.Array.GetLength(1) != matrix2.Array.GetLength(0))
        {
            throw new ArgumentException("Incorrect matrix sizes:(");
        }

        var threads = new Thread[Math.Min(matrix1.Array.GetLength(0), Environment.ProcessorCount)];
        var result = new int[matrix1.Array.GetLength(0), matrix2.Array.GetLength(1)];
        var chunkSize = (matrix1.Array.GetLength(0) / threads.Length) + 1;
        for (int i = 0; i < threads.Length; i++)
        {
            var currentI = i;
            threads[i] = new Thread(() =>
            {
                for (var j = currentI * chunkSize; j < (currentI + 1) * chunkSize && j < matrix1.Array.GetLength(0); j++)
                {
                    for (var k = 0; k < matrix2.Array.GetLength(1); k++)
                    {
                        for (var l = 0; l < matrix1.Array.GetLength(1); l++)
                        {
                            result[j, k] += matrix1.Array[j, l] * matrix2.Array[l, k];
                        }
                    }
                }
            });
        }

        foreach (var thread in threads)
        {
            thread.Start();
        }

        foreach (var thread in threads)
        {
            thread.Join();
        }

        return new Matrix(result);
    }

    /// <summary>
    /// Method for generating a matrix with random values.
    /// </summary>
    /// <returns>Matrix.</returns>
    public static Matrix GenerateMatrix(int rows, int columns)
    {
        var randomizer = new Random();
        var resultArray = new int[rows, columns];
        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < columns; j++)
            {
                resultArray[i, j] = randomizer.Next(50);
            }
        }

        return new Matrix(resultArray);
    }

    /// <summary>
    /// Method for checking two matrices for equality.
    /// </summary>
    /// <returns>bool.</returns>
    public static bool AreMatricesEqual(Matrix matrix1, Matrix matrix2)
    {
        if (matrix1.Array.GetLength(0) != matrix2.Array.GetLength(0) || matrix1.Array.GetLength(1) != matrix2.Array.GetLength(1))
        {
            return false;
        }

        for (var i = 0; i < matrix1.Array.GetLength(0); i++)
        {
            for (var j = 0; j < matrix2.Array.GetLength(1); j++)
            {
                if (matrix1.Array[i, j] != matrix2.Array[i, j])
                {
                    return false;
                }
            }
        }

        return true;
    }
}