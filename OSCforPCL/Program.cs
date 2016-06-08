using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Net;
using System.Linq;

namespace OSCforPCL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string host = "localhost";
            int port = 8006;
            string address = "/tempo/raw";
            float value = 121.3f;

            OSCMessage message = new OSCMessage(address, value);
            OSCPacket newMessage = OSCPacket.Parse(message.Bytes);

            List<OSCMessage> messages = new List<OSCMessage>();
            OSCBundle bundle = new OSCBundle(message);

            OSCPacket newBundleMessage = OSCPacket.Parse(bundle.Bytes);

            Socket client = new Socket(SocketType.Dgram, ProtocolType.Udp);
            EndPoint endPoint = new IPEndPoint(IPAddress.Loopback, port);
            SocketAsyncEventArgs eventArgs = new SocketAsyncEventArgs();
            eventArgs.SetBuffer(bundle.Bytes, 0, bundle.Bytes.Length);
            eventArgs.RemoteEndPoint = endPoint;
            client.SendToAsync(eventArgs);
        }
    }
}