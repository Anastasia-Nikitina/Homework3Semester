using System;
using System.IO;
using static test2.CalcCheckSum;

namespace test2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                throw new ArgumentException("Enter the directory path");
            }
            BitConverter.ToString(OneThreadCalculation(args[0]));
            MultThreadCalculation(args[0]);
            Comparison(args[0]);


        }
    }
}