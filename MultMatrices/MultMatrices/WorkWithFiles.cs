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
            string[] strMatrix = File.ReadAllLines(path);
            int m = strMatrix.Length;
            StringBuilder str = new StringBuilder (strMatrix[0]);
            for (int i = 1; i < strMatrix.Length; i++)
            {
                Console.WriteLine(str);
                str.Append(strMatrix[i]);
            }
            var numbers = str.ToString().Split(' ');
            var n = numbers.Length / m;
            var arr = new int[m, n];
            int k = 0;
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
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
            var strArr = new string[matrix.Rows];
            for (int i = 0; i < matrix.Rows; i++)
            {
                for (int j = 0; j < matrix.Columns; j++)
                {
                    strArr[i] += matrix.Array[i, j] + " ";
                }
            }
            File.WriteAllLines(path, strArr);
        }
    }
}