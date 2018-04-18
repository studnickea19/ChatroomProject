using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Client
    {
        TcpClient clientSocket;
        NetworkStream stream;
        bool keepAlive;

        public Client(string IP, int port)
        {
            clientSocket = new TcpClient();
            clientSocket.Connect("127.0.0.1", port);
            stream = clientSocket.GetStream();
        }

        public void Run()
        {
            keepAlive = true;
            while (keepAlive == true)
            {
                Send();
                Recieve();
              
            }
        }

        public void Send()
        { 
            string messageString = UI.GetInput();
            byte[] message = Encoding.ASCII.GetBytes(messageString);
            stream.Write(message, 0, message.Count());
        }
        public void Recieve()
        {
            byte[] recievedMessage = new byte[256];
            stream.Read(recievedMessage, 0, recievedMessage.Length);
            UI.DisplayMessage(Encoding.ASCII.GetString(recievedMessage));
        }
    }
}
