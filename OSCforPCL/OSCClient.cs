using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace OSCforPCL
{
    public class OSCClient
    {
        public string Host { get; set; }
        public int? Port { get; set; }
        private UdpClient udpClient;

        public OSCClient()
        {
            udpClient = new UdpClient();
        }

        public OSCClient(string host, int port) : this()
        {
            Host = host;
            Port = port;
        }

        public async void Send(OSCPacket packet)
        {
            if(Host != null && Port != null)
            {
                await Send(packet, Host, Port.Value);
            }
            else
            {
                throw new InvalidOperationException("No destination was supplied");
            }
        }

        public async Task<int> Send(OSCPacket packet, string hostname, int port)
        {
            return await udpClient.SendAsync(packet.Bytes, packet.Bytes.Length, hostname, port);
        }
    }
}
