using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Net;
using System.Linq;
using System;

namespace OSCforPCL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            OSCServer server = new OSCServer(9000);
            server.DefaultOnMessageReceived += Server_OnMessageReceived;
            server.AddressOnMessageReceived.Add("/arsehole", (sender, messageArgs) =>
            {
                Console.Out.WriteLine("special " + messageArgs.Message);
            });
            OSCClient client = new OSCClient("127.0.0.1", 9000);
            OSCBundle bundle = new OSCBundle(DateTime.Now + TimeSpan.FromSeconds(3), new OSCMessage("/arsehole", "Sent " + DateTime.Now));
            client.Send(bundle);
            while (true)
            {

            }
        }

        private static void Server_OnMessageReceived(object sender, OSCMessageReceivedArgs e)
        {
            OSCMessage message = e.Message;
            PrintMessage(message);
        }
        

        private static void PrintMessage(OSCMessage message)
        {
            Console.Out.WriteLine(message.ToString());
        }
        
    }
}