using System.Net.Sockets;
using NetworkingLib.Common;
using NetworkingLib.TCPServer;

void HandleConnection(object? that, TcpClient client)
{
    Console.WriteLine("Client connected!");
}

void HandleDisconnection(object? that, TcpClient client)
{
    Console.WriteLine("Client disconnected!");
}

void HandleData(object? that, (TcpClient client, Packet data, int read) args)
{
    Console.WriteLine("Packet ID: " + args.data.ReadInt());
}

var mServer = new TcpServer(1337, 2048);
mServer.OnDataReceived += HandleData;
mServer.OnConnected += HandleConnection;
mServer.OnDisconnected += HandleDisconnection;

await mServer.StartAsync();
