using System;
using System.IO;
using System.Text;

namespace MultMatrices
{
    /// <summary>
    /// Class for functions working with files
    /// </summary>
    public static class WorkWithFiles
    {
        /// <summary>
        /// Method for reading matrix from file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Matrix ReadMatrix(string path)
        {
            var strMatrix = File.ReadAllLines(path);
            var m = strMatrix.Length;
            var str = new StringBuilder (strMatrix[0]);
            for (var i = 1; i < strMatrix.Length; i++)
            {
                str.Append(strMatrix[i]);
            }
            var numbers = str.ToString().Split(' ');
            var n = numbers.Length / m;
            var arr = new int[m, n];
            var k = 0;
            for (var i = 0; i < m; i++)
            {
                for (var j = 0; j < n; j++)
                {
                    if (numbers[k] == "")
                    {
                        k += 1;
                    }
                    arr[i, j] = Int32.Parse(numbers[k]);
                    k += 1;
                }
            }
            return new Matrix(arr);
        }
        /// <summary>
        /// Method for writing matrix to file
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="path"></param>
        public static void WriteMatrix(Matrix matrix, string path)
        {
            var str = new StringBuilder (matrix.Columns);
            for (var i = 0; i < matrix.Rows; i++)
            {
                for (var j = 0; j < matrix.Columns; j++)
                {
                    str.Append(matrix.Array[i, j] + " ");
                }
                str.Append('\n');
            }
            File.WriteAllText(path, str.ToString());
        }
    }
}