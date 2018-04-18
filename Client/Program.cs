using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            //string thisIPAddress = HttpContent.Current.Request.UserHostAddress.ToString();
            Client client = new Client("127.0.0.1", 9999);
            //client.Send();
            //client.Recieve();
            client.Run();
            Console.ReadLine();
        }
    }
}
