using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Message
    {
        public ServerClient sender;
        private string body;
        public string UserName;
        public Message(ServerClient Sender, string Body)
        {
            sender = Sender;
            this.body = sender.userName + ": " + body;
            UserName = sender.userName;
        }

        public string Body
        {
            get { return body; }
            set
            {
                if (body != value)
                {
                    Body = value;
                    
                }
            }
        }

      
    }
}
