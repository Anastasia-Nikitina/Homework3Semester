namespace MultMatrices;

using System.IO;
using System.Text;

/// <summary>
/// Class for functions working with files.
/// </summary>
public static class WorkWithFiles
{
    /// <summary>
    /// Method for reading matrix from file.
    /// </summary>
    /// <returns>Matrix.</returns>
    public static Matrix ReadMatrix(string path)
    {
        var stringOfMatrix = File.ReadAllLines(path);
        var rows = stringOfMatrix.Length;
        var matrixInString = new StringBuilder(stringOfMatrix[0]);
        for (var i = 1; i < stringOfMatrix.Length; i++)
        {
            matrixInString.Append(stringOfMatrix[i]);
        }

        var numbers = matrixInString.ToString().Split(' ');
        var columns = numbers.Length / rows;
        var array = new int[rows, columns];
        var counter = 0;
        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < columns; j++)
            {
                if (numbers[counter] == string.Empty)
                {
                    counter += 1;
                }

                array[i, j] = int.Parse(numbers[counter]);
                counter += 1;
            }
        }

        return new Matrix(array);
    }

    /// <summary>
    /// Method for writing matrix to file.
    /// </summary>
    public static void WriteMatrix(Matrix matrix, string path)
    {
        var resultString = new StringBuilder(matrix.Array.GetLength(1));
        for (var i = 0; i < matrix.Array.GetLength(0); i++)
        {
            for (var j = 0; j < matrix.Array.GetLength(1); j++)
            {
                resultString.Append(matrix.Array[i, j] + " ");
            }

            resultString.Append('\n');
        }

        File.WriteAllText(path, resultString.ToString());
    }
}