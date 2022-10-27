using System.Net;
using ProgramServer;

if (args.Length != 2)
{
    Console.WriteLine("Incorrect arguments. Expected ip and port");
    return;
}
var server = new Server(IPAddress.Parse(args[0]), int.Parse(args[1]));
var serverStart = server.Start();
Console.WriteLine("Enter 'stop' to stop the server");
var stopCommand = "";
while (stopCommand != "stop")
{
    stopCommand = Console.ReadLine();
}

server.Stop();
await serverStart;