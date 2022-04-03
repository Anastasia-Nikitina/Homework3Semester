namespace  MyNUnit;

public static class Program
{
    static void Main(string[] args)
    {
        
        var path = args[0];
        if (!Directory.Exists(path))  throw new ArgumentException("No directory found");
        MyNUnit myNUnit = new();
        myNUnit.PrintInfo(path);
        
    }
 
}