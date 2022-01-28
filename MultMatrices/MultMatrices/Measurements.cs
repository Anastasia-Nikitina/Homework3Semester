using System;
using System.Diagnostics;
using System.Linq;
using static MultMatrices.Matrix;
using static System.Math;

namespace MultMatrices;
    /// <summary>
    /// Class for measurement functions 
    /// </summary>
    public static class Measurements
    {
        /// <summary>
        /// Method for measuring the multiplication time of two matrices
        /// </summary>
        private static long MeasureTime(Matrix matrix1, Matrix matrix2, Func<Matrix, Matrix, Matrix> fun)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            fun(matrix1, matrix2);
            stopWatch.Stop();
            return stopWatch.ElapsedMilliseconds;
        }
        
        /// <summary>
        /// Method for calculating average time and standard deviation
        /// </summary>
        public static void ResMeasurements(Func<Matrix, Matrix, Matrix> fun)
        {
            for (var size = 100; size <= 1900; size += 300)
            {
                long sumTime = 0;
                var arrayTime = new long[16];

                for (var i = 0; i <= 15; i++)
                {
                    var matrix1 = GenerateMatrix(size, size);
                    var matrix2 = GenerateMatrix(size, size);
                    var time = MeasureTime(matrix1, matrix2, fun);
                    arrayTime[i] = time;
                    sumTime += time;
                }
                var average = sumTime / 15;
                var standDev = arrayTime.Sum(t => Pow(t - average, 2));

                standDev /= 15;

                Console.WriteLine($"Measurement of matrix {size}x{size} multiplication time :");
                Console.WriteLine(
                    $"Average time: {average} ms, standard deviation: {Round(Sqrt(standDev), 5)} ms\n");
            }
        }
    }