namespace MultMatrices;

using System;
using System.Diagnostics;
using System.Linq;
using static Matrix;
using static System.Math;

/// <summary>
/// Class for measurement functions.
/// </summary>
public static class Measurements
{
    /// <summary>
    /// Method for calculating average time and standard deviation.
    /// </summary>
    public static void ResultMeasurements(Func<Matrix, Matrix, Matrix> fun)
    {
        const int maxSize = 1900;
        const int numberOfMeasurements = 15;
        const int initialSize = 100;
        const int delta = 300;

        for (var size = initialSize; size <= maxSize; size += delta)
        {
            long sumTime = 0;
            var arrayTime = new long[16];

            for (var i = 0; i <= numberOfMeasurements; i++)
            {
                var matrix1 = GenerateMatrix(size, size);
                var matrix2 = GenerateMatrix(size, size);
                var time = MeasureTime(matrix1, matrix2, fun);
                arrayTime[i] = time;
                sumTime += time;
            }

            var average = sumTime / 15;
            var standDev = arrayTime.Sum(t => Pow(t - average, 2));

            standDev /= numberOfMeasurements;

            Console.WriteLine($"Measurement of matrix {size}x{size} multiplication time :");
            Console.WriteLine(
                $"Average time: {average} ms, standard deviation: {Round(Sqrt(standDev), 5)} ms\n");
        }
    }

    /// <summary>
    /// Method for measuring the multiplication time of two matrices.
    /// </summary>
    private static long MeasureTime(Matrix matrix1, Matrix matrix2, Func<Matrix, Matrix, Matrix> fun)
    {
        var stopWatch = new Stopwatch();
        stopWatch.Start();
        fun(matrix1, matrix2);
        stopWatch.Stop();
        return stopWatch.ElapsedMilliseconds;
    }
}