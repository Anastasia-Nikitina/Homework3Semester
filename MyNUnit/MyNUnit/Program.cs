if (args.Length != 1)
{
    Console.WriteLine("Enter one argument: the path to the directory containing the dll files");
    return;
}
var path = args[0];
if (!Directory.Exists(path))
{
    Console.WriteLine("No directory found");
    return;
}
var myNUnit = new MyNUnit.MyNUnit();
myNUnit.RunAndPrintInfo(path);

