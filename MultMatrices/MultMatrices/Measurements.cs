using System;
using System.Diagnostics;
using static MultMatrices.Matrix;
using static System.Math;

namespace MultMatrices
{
    /// <summary>
    /// Class for measurement functions 
    /// </summary>
    public static class Measurements
    {
        /// <summary>
        /// Method for measuring the multiplication time of two matrices
        /// </summary>
        /// <param name="matrix1"></param>
        /// <param name="matrix2"></param>
        /// <param name="fun"></param>
        /// <returns></returns>
        private static long MeasureTime(Matrix matrix1, Matrix matrix2, Func<Matrix, Matrix, Matrix> fun)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            fun(matrix1, matrix2);
            stopWatch.Stop();
            return stopWatch.ElapsedMilliseconds;
        }
        /// <summary>
        /// Method for calculating average time and standard deviation
        /// </summary>
        /// <param name="fun"></param>
        public static void ResMeasurements(Func<Matrix, Matrix, Matrix> fun)
        {
            for (int size = 100; size <= 1900; size += 300)
            {
                long sumTime = 0;
                long [] arrTime = new long[16];

                for (int i = 0; i <= 15; i++)
                {
                    Matrix matrix1 = GenerateMatrix(size, size);
                    Matrix matrix2 = GenerateMatrix(size, size);
                    long time = MeasureTime(matrix1, matrix2, fun);
                    arrTime[i] = time;
                    sumTime += time;
                }
                var average = sumTime /= 15;
                double standDev = 0;
                foreach (var t in arrTime)
                {
                    standDev += Pow(t - average, 2);
                }

                standDev /= 15;

                Console.WriteLine($"Measurement of matrix {size}x{size} multiplication time :");
                Console.WriteLine(
                    $"Average time: {(double) average} ms, standard deviation: {Round(Sqrt(standDev), 5)} ms\n");
            }
        }
    }
}