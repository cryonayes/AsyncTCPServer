using System.Net;
using System.Net.Sockets;
using AsyncTCPServer.Common;

namespace AsyncTCPServer.TCPServer
{
    public class TcpServer
    {
        private readonly TcpListener _listener;
        private readonly int _receiveBufferSize;
        
        public event EventHandler<TcpClient>? OnConnected;
        public event EventHandler<TcpClient>? OnDisconnected;
        public event EventHandler<(TcpClient client, Packet packet)>? OnDataReceived;
        
        public bool IsListening = false;

        public TcpServer(int port, int receiveBufferSize)
        {
            _listener = new TcpListener(IPAddress.Any, port);
            _receiveBufferSize = receiveBufferSize;
        }

        public void Stop()
        {
            _listener.Stop();
            IsListening = false;
        }
        
        public async Task StartAsync(CancellationToken token = default)
        {
            _listener.Start();
            IsListening = true;
            
            while (!token.IsCancellationRequested) {
                try {
                    var client = await _listener.AcceptTcpClientAsync(token);
                    Task.Run(() => HandleClientAsync(client));
                }
                catch (Exception ex) {
                    Console.WriteLine("Error: {0}", ex.Message);
                }
            }
        }

        private async Task HandleClientAsync(TcpClient client)
        {
            OnConnected?.Invoke(this, client);
            
            var stream = client.GetStream();
            var buffer = new byte[_receiveBufferSize];
            var packet = new Packet();

            while (true)
            {
                try
                {
                    var bytesRead = await stream.ReadAsync(buffer, 0, _receiveBufferSize);
                    if (bytesRead == 0)
                        break;
                    packet.SetBytes(buffer);
                    OnDataReceived?.Invoke(this, (client, packet));
                    packet.Reset();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: {0}", ex.Message);
                    break;
                }
            }
            OnDisconnected?.Invoke(this, client);
            client.Close();
        }
    }
}
