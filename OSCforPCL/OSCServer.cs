using OSCforPCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace OSCforPCL
{
    public class OSCServer : IDisposable
    {
        UdpClient client;
        public event EventHandler<OSCMessageReceivedArgs> DefaultOnMessageReceived;
        public Dictionary<string, EventHandler<OSCMessageReceivedArgs>> AddressOnMessageReceived { get; set; }

        public OSCServer(int port)
        {
            AddressOnMessageReceived = new Dictionary<string, EventHandler<OSCMessageReceivedArgs>>();
            client = new UdpClient(port);
            Task.Factory.StartNew(async () =>
            {
                while (client != null)
                {
                    while (client.Available == 0)
                    {
                        await Task.Delay(10);
                    }
                    UdpReceiveResult result = await client.ReceiveAsync();
                    OSCPacket packet = OSCPacket.Parse(result.Buffer);
                    if(packet is OSCBundle)
                    {
                        OSCBundle bundle = packet as OSCBundle;
                        OnBundleReceived(bundle);
                    }
                    else
                    {
                        OSCMessage message = packet as OSCMessage;
                        OnMessageReceived(message);
                    }
                }
            });
        }
        
        public void Dispose()
        {
            client.Dispose();
        }

        private void OnBundleReceived(OSCBundle bundle)
        {
            foreach(OSCPacket packet in bundle.Contents)
            {
                if(packet is OSCBundle)
                {
                    OSCBundle subBundle = packet as OSCBundle;
                    OnBundleReceived(subBundle);
                }
                else
                {
                    OSCMessage message = packet as OSCMessage;
                    OnMessageReceived(message);
                }
            }
        }

        private void OnMessageReceived(OSCMessage message)
        {
            if(AddressOnMessageReceived.ContainsKey(message.Address.Contents))
            {
                AddressOnMessageReceived[message.Address.Contents].Invoke(this, new OSCMessageReceivedArgs(message));
            }
            else
            {
                DefaultOnMessageReceived?.Invoke(this, new OSCMessageReceivedArgs(message));
            }
        }
    }
    
    public class OSCMessageReceivedArgs
    {
        public OSCMessage Message { get;}
        public OSCMessageReceivedArgs(OSCMessage message)
        {
            Message = message;
        }
    }
}
