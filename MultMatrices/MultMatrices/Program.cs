using static MultMatrices.WorkWithFiles;
using static  MultMatrices.Matrix;

namespace MultMatrices
{
    static class Program
    {
        static void Main(string[] args)
        {
             Matrix matrix1 = ReadMatrix(args[0]);
             Matrix matrix2 = ReadMatrix(args[1]);
             Matrix res = ParMultMatrix(matrix1, matrix2);
             WriteMatrix(res, args[2]);
        }
    }
}