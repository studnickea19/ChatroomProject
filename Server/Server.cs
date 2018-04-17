using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class Server
    {
        public static ServerClient client;
        TcpListener server;
        string defaultServerIP;
        int defaultServerPort;
        bool keepAlive;

        public Server()
        {
            defaultServerIP = "127.0.0.1";
            defaultServerPort = 9999;
            keepAlive = true;
            server = new TcpListener(IPAddress.Parse(defaultServerIP), defaultServerPort);
            server.Start();
        }
        public void Run()
        {
            while (keepAlive == true)
            {
                AcceptClient();
                string message = client.Recieve();
                Respond(message);
            }
        }
        private void AcceptClient()
        {
            TcpClient clientSocket = default(TcpClient);
            clientSocket = server.AcceptTcpClient();
            Console.WriteLine("Connected");
            NetworkStream stream = clientSocket.GetStream();
            client = new ServerClient(stream, clientSocket);
        }
        private void Respond(string body)
        {
             client.Send(body);
        }
    }
}
