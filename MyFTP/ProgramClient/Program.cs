using System;
using System.IO;
using System.Net;
using System.Threading;
using ProgramClient;

if (args.Length != 4)
{
    Console.WriteLine(
        "Incorrect number of arguments. Expected ip, port, number of request (1 for List or 2 for Get) and path");
    return;
}

if (!int.TryParse(args[1], out var port) || port is < 0 or > 65535)
{
    Console.WriteLine("Incorrect port. Expected number from 0 to 65535 ");
}

if (!IPAddress.TryParse(args[0], out var ip))
{
    Console.WriteLine("Incorrect ip");
}

if (!int.TryParse(args[2], out var numberRequest) || numberRequest is not (1 or 2))
{
    Console.WriteLine("Incorrect number of request. Expected 1 for List or 2 for Get");
}


var token = new CancellationToken();
var client = new Client(ip, port);
switch (numberRequest)
{
    case 1:
        var result1 = await client.List(args[3], token);
        Console.WriteLine(result1.ToString());
        break;
    case 2:
        var destination = new FileStream(args[3],FileMode.Open);
        var result2 = await client.Get(args[3], destination, token);
        Console.WriteLine(result2);
        break;
    default:
        Console.WriteLine("Incorrect argument");
        return;
}
