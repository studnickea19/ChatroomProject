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
        Queue<Message> chatroom;
        private Object messageThread = new Object();
        ISubscriber subscriber;
        
        public Server(ISubscriber subscriber)
        {

            defaultServerIP = "127.0.0.1";
            defaultServerPort = 9999;
            keepAlive = true;
            server = new TcpListener(IPAddress.Parse(defaultServerIP), defaultServerPort);
            chatroom = new Queue<Message>();
            userDictionary = new Dictionary<string, ServerClient>();
            this.subscriber = subscriber;
            server.Start();
        }

        public void Run()
        {
            while (keepAlive)
            {
                Parallel.Invoke(() =>
                {
                    Console.WriteLine("Listening for Client");
                    Task.Run(() => AcceptClient());
                },
                () =>
                {                    
                    Console.WriteLine("Waiting for message");           
                    Task.Run(() => Listen());
                },
                () =>
                {
                    Respond();
                    Console.WriteLine("Message printed");
                });
                    
                                  
            }
        }

        private void AcceptClient()
        {
            while(true)
            {
                try
                {
                    TcpClient clientSocket = default(TcpClient);
                    clientSocket = server.AcceptTcpClient();
                    Console.WriteLine("Connected");
                    NetworkStream stream = clientSocket.GetStream();
                    client = new ServerClient(stream, clientSocket);
                    AddUser(client);
                }

                catch(Exception e)
                {
                    string message = "No client yet";
                    subscriber.WriteMessage(message);
                }
                
            }
            
        }
        private void Respond()
        {
            try
            {
                Message message = chatroom.Dequeue();
                lock (messageThread)
                {
                    foreach (KeyValuePair<string, ServerClient> entry in userDictionary)
                    {
                        client.Send(message.Body);
                    }
                }           
                
            }

            catch(Exception e)
            {
                Console.WriteLine("No messages yet.");
            }
            
        }

        public void AddUser(ServerClient client)
        {
            string userName = client.Recieve();
            client.userName = userName.Trim('\0');
            string joinNotice = client.userName + " has joined the chatroom";
            userDictionary.Add(client.userName, client);
            Message message = new Message(client, joinNotice);
            chatroom.Enqueue(message);
        }

        public void Listen()
        {
            if (userDictionary.Count == 0)
            {
                Task waitForClient = Task.Run(() => AcceptClient());
                waitForClient.Wait();
            }
            else
            {
                try
                {
                    while (true)
                    {
                        string messageString = client.Recieve();
                        Message message = new Message(client, messageString);
                        foreach (KeyValuePair<string, ServerClient> entry in userDictionary)
                        {
                            chatroom.Enqueue(message);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("No messages");
                }
            }
            
        }
    }
}
