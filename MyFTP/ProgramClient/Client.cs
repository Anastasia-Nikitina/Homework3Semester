namespace ProgramClient;

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Class for MyFTP client
/// </summary>
public class Client
{
    private readonly IPAddress _ip;
    private readonly int _port;

    /// <summary>
    /// Constructor of client
    /// </summary>
    public Client(IPAddress ip, int port)
    {
        _ip = ip;
        _port = port;
    }
    
    /// <summary>
    /// Returns files and directories on server
    /// </summary>
    public async Task<List<(string name, bool isDir)>> List(string path, CancellationToken token)
    {
        using var client = new TcpClient();
        await client.ConnectAsync(_ip, _port, token);
        await using var stream = client.GetStream();
        await using var writer = new StreamWriter(stream){AutoFlush = true};
        using var reader = new StreamReader(stream);
        await writer.WriteLineAsync("1 " + path);
        var files = new List<(string, bool)>();
        var response = await reader.ReadLineAsync();
        if (response == null) throw new NullReferenceException();
        var splitResponse = response.Split(' ');
        if (splitResponse[0] == "-1")
        {
            throw new FileNotFoundException();
        }
        
        for (var i = 1; i < splitResponse.Length; i += 2)
        {
            files.Add((splitResponse[i], bool.Parse(splitResponse[i + 1])));
        }
        return files;
    } 

    /// <summary>
    /// Gets file from server
    /// </summary>
    public async Task <long> Get(string path, FileStream destination, CancellationToken token)
    {
        using var client = new TcpClient();
        await client.ConnectAsync(_ip, _port, token);
        await using var stream = client.GetStream();
        await using var writer = new StreamWriter(stream) {AutoFlush = true};
        await writer.WriteLineAsync("2 " + path);
        using var reader = new StreamReader(stream);
        var data = long.Parse(await reader.ReadLineAsync() ?? string.Empty);
        if (data == -1)
        {
            throw new FileNotFoundException();
        }
        await stream.CopyToAsync(destination, token);
        return data;
    }
}