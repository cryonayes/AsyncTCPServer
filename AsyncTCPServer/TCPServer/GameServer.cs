using System.Net.Sockets;
using System.Text;
using AsyncTCPServer.Common;

namespace AsyncTCPServer.TCPServer;

public class GameServer
{
    private readonly TcpServer _server;
    
    private readonly List<TcpClient> _clients = new();
    private readonly object _clientLock = new();
    
    public GameServer(int port, int receiveSize)
    {
        _server = new TcpServer(port, receiveSize);
        _server.OnConnected += OnConnected;
        _server.OnDisconnected += OnDisconnected;
        _server.OnDataReceived += OnDataReceived;
    }
    
    public async Task StartAsync()
    {
        await _server.StartAsync();
    }
    
    private void OnDataReceived(object? sender, (TcpClient client, Packet packet) args)
    {
        var clientId = _clients.IndexOf(args.client);
        var stream = args.client.GetStream();
        
        stream.Write(Encoding.ASCII.GetBytes(clientId.ToString()));
    }
    
    private void OnDisconnected(object? sender, TcpClient client)
    {
        lock (_clientLock)
            _clients.Remove(client);
    }
    
    private void OnConnected(object? sender, TcpClient client)
    {
        lock (_clientLock)
            _clients.Add(client);
    }
    
}