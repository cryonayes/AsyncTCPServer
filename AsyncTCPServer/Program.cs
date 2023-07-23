using AsyncTCPServer.TCPServer;

var mServer = new GameServer(1337, 2048);

await mServer.StartAsync();