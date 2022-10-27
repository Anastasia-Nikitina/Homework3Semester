namespace ProgramServer;

using System.Net;
using System.Net.Sockets;
using System.Text;

/// <summary>
/// Class for MyFTP Server
/// </summary>
public class Server
{
    private readonly List<Task> _requests = new();
    private readonly CancellationTokenSource _tokenSource = new();
    private readonly TcpListener _server;
     
    /// <summary>
    /// Constructor of server
    /// </summary>
    public Server(IPAddress ip, int port)
    {
        _server = new TcpListener(ip, port);
    }
    
    /// <summary>       
    /// Returns files and directories from server to client
    /// </summary>
    private async Task List(string path, StreamWriter writer)
    {
        if (!Directory.Exists(path))
        {
            await writer.WriteLineAsync("-1");
            return;
        }
        var files = Directory.GetFiles(path);
        var directories = Directory.GetDirectories(path);
        var response = new StringBuilder((files.Length + directories.Length).ToString());
        foreach (var file in files)
        {
            response.Append($" {file} false");
        }
        foreach (var directory in directories)
        {
            response.Append($" {directory} true");
        }
        await writer.WriteLineAsync(response.ToString());
    }

    /// <summary>
    /// Gets file from server
    /// </summary>
    private async Task Get(string path, StreamWriter writer)
    {
        if (!File.Exists(path))
        {
            await writer.WriteLineAsync("-1");
            return;
        }
        await using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
        await writer.WriteLineAsync(fileStream.Length + " ");
        await fileStream.CopyToAsync(writer.BaseStream);
    }

    /// <summary>
    /// Executes listening of client
    /// </summary>
    private async Task Executing(TcpClient client)
    {
        await using var stream = client.GetStream();
        await using var writer = new StreamWriter(stream) {AutoFlush = true};
        using var reader = new StreamReader(stream);
        var request = (await reader.ReadLineAsync())?.Split(' ');
        if (request != null)
        {
            switch (request[0])
            {
                case "1":
                    await List(request[1], writer);
                    break;
                case "2":
                    await Get(request[1], writer);
                    break;
                default:
                    await writer.WriteAsync("Incorrect request");
                    break;
            }
        }
    }

    /// <summary>
    /// Starts server
    /// </summary>
    public async Task Start()
    {
        _server.Start();
        while (!_tokenSource.Token.IsCancellationRequested)
        {
            var client = await _server.AcceptTcpClientAsync(_tokenSource.Token);
            _requests.Add(Task.Run(() => Executing(client)));
        }
        Task.WaitAll(_requests.ToArray());
        _server.Stop();
    }
    
    /// <summary>
    /// Stops server
    /// </summary>
    public void Stop()
    {
        _tokenSource.Cancel();
    }
}