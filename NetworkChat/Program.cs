using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace NetworkChat
{
    class Program
    {
        private Client Client;
        private Server Server;
        
        private async static Task Main(string[] args)
        {
            if (args.Length == 2)
            {
                var client = new Client(args[0], int.Parse(args[1]));
                client.Go();
            }
            else
            {
                var listener = new Server(int.Parse(args[0]));
                listener.Go();
            }
        }
        
    }

}    