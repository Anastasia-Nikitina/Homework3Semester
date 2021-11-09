using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace NetworkChat
{
    public class Client
    {
        private TcpClient client;
        private string _ip;
        private int _port;


        public Client(string ip, int port)
        {
            client = new TcpClient(ip, port);
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
            await client.ConnectAsync(_ip, _port);
            string text = "";
            await Read();
            await Write();
        }
        

    }

}