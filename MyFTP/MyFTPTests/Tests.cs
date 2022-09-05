namespace MyFTPTests;

using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using ProgramClient;
using ProgramServer;


[TestFixture]
public class Tests
{
    private const string Ip = "127.0.0.1";
    private const int Port = 8888;
    private Client _client;
    private Server _server;
    private readonly CancellationToken _token = new();


    [SetUp]
    public void Setup()
    {
        _client = new Client(IPAddress.Parse(Ip), Port);
        _server = new Server(IPAddress.Parse(Ip), Port);
        _server.Start();
    }
    
    [TearDown]
    public void Teardown()
        => _server.Stop();
    
    [Test]    
    public async Task TestForListDirectory()
    {
        const string path = "../../../Test";
        var result = await _client.List(path, _token);
        Assert.AreEqual((Path.Combine(path, "Directory"), true) , result[0]);
    }
    
    [Test]    
    public async Task TestForListFile()
    {
        const string path = "../../../Test/Directory";
        var result = await _client.List(path, _token);
        Assert.AreEqual((Path.Combine(path, "file1.txt"), false) , result[0]);
    }
    
    [Test]
    public void TestForListIncorrectNameOfFile()
    {
        Assert.ThrowsAsync<FileNotFoundException>(() => _client.List("hehe.txt", _token));
    }
    
    
    [Test]
    public void TestForGetIncorrectNameOfFile()
    {
        Assert.ThrowsAsync<FileNotFoundException>(() => _client.Get("hehe.txt", null, _token));
    }
    
    [Test]
    public async Task TestForGet()
    {
        var path = "../../../Test/Directory";
        var destination = Path.Combine(path, "/fileForCopy.txt");
        var pathForGet = Path.Combine(path, "/file1.txt");
        var expect = await File.ReadAllBytesAsync(pathForGet, _token);
        await using var fileStream = new FileStream(destination, FileMode.OpenOrCreate); 
        await _client.Get(pathForGet, fileStream, _token);
        var result = await File.ReadAllBytesAsync(destination, _token);
        Assert.AreEqual(expect, result);
    }
}