using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace Server
{
    class Server
    {
        public static ServerClient client;
        TcpListener server;
        string defaultServerIP;
        int defaultServerPort;
        bool keepAlive;
        public Dictionary<string, ServerClient> userDictionary;
        public Server()
        {

            defaultServerIP = "127.0.0.1";
            defaultServerPort = 9999;
            keepAlive = true;
            server = new TcpListener(IPAddress.Parse(defaultServerIP), defaultServerPort);
            server.Start();
            userDictionary = new Dictionary<string, ServerClient>();
        }

        public async void Run()
        {
            while (keepAlive)
            {
                Console.WriteLine("Creating Client");
                await AcceptClient();
                Console.WriteLine("Waiting for message");
                string message = client.Recieve();
                await Respond(message);
                Console.WriteLine("Message printed");                                  
            }
        }

        private Task AcceptClient()
        {
                return Task.Factory.StartNew(() =>
                {
                    TcpClient clientSocket = default(TcpClient);
                    clientSocket = server.AcceptTcpClient();
                    Console.WriteLine("Connected");
                    NetworkStream stream = clientSocket.GetStream();                   
                    client = new ServerClient(stream, clientSocket);
                    AddUser(client);
                    
                });
        }
        private Task Respond(string body)
        {
            return Task.Factory.StartNew(() =>
            {
                client.Send(body);
            });
        }

        public void AddUser(ServerClient client)
        {
            string userName = client.Recieve();
            userDictionary.Add(userName, client);
            client.Send(userName + "has joined the chatroom");
        }


        //Parallel.Invoke(() =>
          //  {
            //    AcceptClient();
            //},
            //()  =>
            //{

            //string message = client.Recieve();
            //Respond(Message);
        //});
    }
}
