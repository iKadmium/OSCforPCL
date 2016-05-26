# OSCforPCL
A relatively lightweight OSC library for PCL

Usage is pretty simple - 
```
OSCMessage messageOne = new OSCMessage("/address1", 5.0f); //one float argument
OSCMessage messageTwo = new OSCMessage("/address2", 5.0f, 3.2f, "attributes"); //two floats and a string
```
Bundles are supported.
```
OSCBundle bundle = new OSCBundle(messageOne, messageTwo);
```
In order to then send that to a network device:
```
Socket client = new Socket(SocketType.Dgram, ProtocolType.Udp); //new UDP socket
EndPoint endPoint = new IPEndPoint(IPAddress.Loopback, 8000); //send to localhost, port 8000
SocketAsyncEventArgs eventArgs = new SocketAsyncEventArgs();
eventArgs.RemoteEndPoint = endPoint;
eventArgs.SetBuffer(bundle.Bytes, 0, bundle.Bytes.Length); //you can send a message or a bundle. To send a message, just use message.bytes
client.SendToAsync(eventArgs);
```

Parsing receiving bytes is still a work in progress.
