namespace  MyNUnit;

internal static class Program
{
    static void Main(string[] args)
    {
        var path = args[0];
        if (!Directory.Exists(path))
        {
            Console.WriteLine("No directory found");
            return;
        }
           
        MyNUnit myNUnit = new();
        myNUnit.PrintInfo(path);
    }
}