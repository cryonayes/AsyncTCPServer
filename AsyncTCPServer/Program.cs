using System.Net.Sockets;
using System.Text;
using AsyncTCPServer.Common;
using AsyncTCPServer.TCPServer;

void HandleConnection(object? that, TcpClient client)
{
    Console.WriteLine("Client connected!");
}

void HandleDisconnection(object? that, TcpClient client)
{
    Console.WriteLine("Client disconnected!");
}

void HandleData(object? that, (NetworkStream client, Packet data) args)
{
    var stringRead = args.data.ReadString();
    if (stringRead == "ping")
        args.client.WriteAsync(Encoding.ASCII.GetBytes("pong"));
}

var mServer = new TcpServer(1337, 2048);


mServer.OnDataReceived += HandleData;
mServer.OnConnected += HandleConnection;
mServer.OnDisconnected += HandleDisconnection;

await mServer.StartAsync();
