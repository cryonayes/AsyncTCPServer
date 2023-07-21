using System.Net.Sockets;
using AsyncTCPServer.Common;
using AsyncTCPServer.TCPServer;
using Xunit;
using Xunit.Abstractions;

namespace AsyncTCPServer.UnitTests;

internal enum PacketId
{
    Ping = 0,
    Pong
}

public class ServerTest
{
    private readonly TcpServer _mServer;
    private readonly ITestOutputHelper output;
    
    public ServerTest(ITestOutputHelper testOutput)
    {
        output = testOutput;
        
        _mServer = new TcpServer(1337, 2048);
        _mServer.OnDataReceived += OnDataReceived; 
        Task.Run(async () => await _mServer.StartAsync());
    }

    private void OnDataReceived(object? sender, (NetworkStream stream, Packet packet) args)
    {
        var packetId = args.packet.ReadInt();
        switch (packetId)
        {
            case (int)PacketId.Ping:
                var packet = new Packet((int)PacketId.Pong);
                packet.Write("pong");
                packet.WriteLength();
                args.stream.Write(packet.ToArray());
                break;
            default:
                output.WriteLine("Unknown packet!");
                break;
        }
    }
    
    [Fact]
    public void TestConnection()
    {
        var client = new TcpClient("127.0.0.1", 1337);
        client.Close();
    }

    [Fact]
    public void TestPing()
    {
        var client = new TcpClient("127.0.0.1", 1337);
        var stream = client.GetStream();
        
        var packet = new Packet((int)PacketId.Ping);
        stream.Write(packet.ToArray());
        packet.Reset();

        var buffer = new byte[64];
        var readStream = stream.Read(buffer, 0, buffer.Length);
        
        packet.SetBytes(buffer);
        var readPacket = packet.ReadInt() + sizeof(int);
        
        Assert.True(readPacket == readStream, "Packet length is invalid!");
        
        if (packet.ReadInt() == (int)PacketId.Pong)
        {
            if (packet.ReadString() != "pong")
                throw new Exception("Ping Pong string invalid!");
        }
        else
            throw new Exception("Ping Pong packet id invalid!");

        client.Close();
    }
}