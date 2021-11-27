using static MultMatrices.WorkWithFiles;
using static  MultMatrices.Matrix;

namespace MultMatrices
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
             var matrix1 = ReadMatrix(args[0]);
             var matrix2 = ReadMatrix(args[1]);
             var res = ParMultMatrix(matrix1, matrix2);
             WriteMatrix(res, args[2]);
        }
    }
}