using System;
using System.Threading;

namespace MultMatrices;
public class Matrix
    {
        public int Rows { get; }
        public int Columns { get; }
        public int[,] Array { get; }
        
        /// <summary>
        /// Constructor of matrices
        /// </summary>
        public Matrix(int[,] matrix)
        {
            Rows = matrix.GetLength(0);
            Columns = matrix.GetLength(1);
            Array = matrix;
        }
        
        /// <summary>
        /// Method for sequential matrix multiplication
        /// </summary>
        public static Matrix SequentialMatrixMultiplication(Matrix matrix1, Matrix matrix2)
        {
            if (matrix1.Columns != matrix2.Rows)
            {
                throw new ArgumentException("Incorrect matrix sizes:(");
                
            }
            var res = new int[matrix1.Rows, matrix2.Columns];
            for (var i = 0; i < matrix1.Rows; i++)
            {
                for (var j = 0; j < matrix2.Columns; j++)
                {
                    for (var k = 0; k < matrix1.Columns; k++)
                    {
                        res[i, j] += matrix1.Array[i, k] * matrix2.Array[k, j];
                    }
                }
            }
            return new Matrix(res);
        }
        
        /// <summary>
        /// Method for parallel matrix multiplication
        /// </summary>
        public static Matrix ParallelMatrixMultiplication(Matrix matrix1, Matrix matrix2)
        {
            if (matrix1.Columns != matrix2.Rows)
            {
                throw new ArgumentException("Incorrect matrix sizes:(");
            }
            var threads = new Thread[Math.Min(matrix1.Rows,Environment.ProcessorCount)];
            var result = new int [matrix1.Rows, matrix2.Columns];
            var chunkSize = matrix1.Rows / threads.Length + 1;
            for (int i = 0; i < threads.Length; i++)
            {
                var currentI = i;
                threads[i] = new Thread(() =>
                {
                    for (var j = currentI * chunkSize; j < (currentI + 1) * chunkSize && j < matrix1.Rows; j++)
                    {
                        for (var k = 0; k < matrix2.Columns; k++)
                        {
                            for (var l = 0; l < matrix1.Columns; l++)
                            {
                                result[j, k] += matrix1.Array[j, l] * matrix2.Array[l, k];
                            }
                        }
                    }
                });
            }
            foreach (var thread in  threads)
            {
                thread.Start();
            }
            foreach (var thread in  threads)
            {
                thread.Join();
            }
            return new Matrix(result);
        }
        
        /// <summary>
        /// Method for generating a matrix with random values
        /// </summary>
        public static Matrix GenerateMatrix(int rows, int columns)
        {
            var rand = new Random();
            var res = new int[rows, columns];
            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < columns; j++)
                {
                    res[i, j] = rand.Next(50);
                }
            }
            return new Matrix(res);
        }
        
        /// <summary>
        /// Method for checking two matrices for equality
        /// </summary>
        public static bool IsMatricesEqual(Matrix matrix1, Matrix matrix2)
        {
            if (matrix1.Rows != matrix2.Rows || matrix1.Columns != matrix2.Columns)
            {
                return false;
            }
            for (var i = 0; i < matrix1.Rows; i++)
            {
                for (var j = 0; j < matrix2.Columns; j++)
                {
                    if (matrix1.Array[i, j] != matrix2.Array[i ,j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }