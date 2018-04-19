﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class ServerClient //: ISubscribable
    {
        NetworkStream stream;
        TcpClient client;
        public string userName;

        public NetworkStream Stream
        {
            get { return stream; }
        }

        public string UserName
        {
           set { userName = value; }   
        }
        public ServerClient(NetworkStream Stream, TcpClient Client)
        {
            stream = Stream;
            client = Client;
        }
        public void Send(string Message)
        {
            byte[] message = Encoding.ASCII.GetBytes(Message);
            stream.Write(message, 0, message.Count());
        }
        public string Recieve()
        {
            byte[] recievedMessage = new byte[256];
            stream.Read(recievedMessage, 0, recievedMessage.Length);
            string recievedMessageString = Encoding.ASCII.GetString(recievedMessage);
            Console.WriteLine(recievedMessageString);
            return recievedMessageString;
        }
    }
}
