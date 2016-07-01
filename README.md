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
int port = 9000;
string address = "127.0.0.1";
OSCClient client = new OSCClient(address, port);
client.Send(bundle);
```
To listen for OSC messages:
```
int port = 9000;
OSCServer server = new OSCServer(port);
//listen to messages sent to a specific address
string oscAddress = "/test";
server.AddressOnMessageReceived.Add(specificAddress, (oscAddress, messageArgs) =>
{
  foreach(IOSCValue argument in messageArgs.Arguments)
  {
    //do something with the argument
    object value = argument.GetValue();
  }
});
//listen to messages sent to any address for which a handler is not already specified
server.DefaultOnMessageReceived += (sender, messageArgs) =>
{
  foreach(IOSCValue argument in messageArgs.Arguments)
  {
    //do something with the argument
    object value = argument.GetValue();
  }
};
```
