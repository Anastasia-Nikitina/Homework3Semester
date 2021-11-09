using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace NetworkChat
{
    public class Server
    {
            private int _port;
            private CancellationTokenSource cancellationTokenSource = new();
            private TcpClient client;
            private TcpListener listener;
            
            public Server(int port)
            {
                listener = new TcpListener(port);
            }


            public async Task Read()
            {
                var stream = client.GetStream();
                var reader = new StreamReader(stream);
                string text = await reader.ReadLineAsync();
                Console.WriteLine(text);
                
            } 
            
            public async Task Write()
            {
            
                var stream = client.GetStream();
                string text = "";
                var writer = new StreamWriter(stream);
                await writer.FlushAsync();
                do
                {
                    text = Console.ReadLine();
                    await writer.WriteLineAsync(text);
                } while (text != "exit");
                client.Close();


            }

            public async Task Go()
            {
                listener.Start();
                while (!cancellationTokenSource.IsCancellationRequested)
                {
                    await Read();
                    await Write();
                }
                listener.Stop();
            }
    }
    
    
}