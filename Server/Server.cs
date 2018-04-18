﻿using System;
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
        private string message;
        public Server()
        {

            defaultServerIP = "127.0.0.1";
            defaultServerPort = 9999;
            keepAlive = true;
            server = new TcpListener(IPAddress.Parse(defaultServerIP), defaultServerPort);
            server.Start();
        }

        public async void Run()
        {
           
            while (keepAlive)
            {
                //Use Task somewhere here
                Parallel.Invoke(() =>
                    {
                        AcceptClient();
                    },
                    () =>
                    {
                        Respond(message);
                    });
            }
        }

        private Task AcceptClient()
        {
            return Task.Run(() =>
            {
                TcpClient clientSocket = default(TcpClient);
                clientSocket = server.AcceptTcpClient();
                Console.WriteLine("Connected");
                NetworkStream stream = clientSocket.GetStream();
                client = new ServerClient(stream, clientSocket);
            });

        }
        private void Respond(string body)
        {
             client.Send(body);
        }
    }
}
