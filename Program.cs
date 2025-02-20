using BalanceX.Handlers;
using System.Net;
using System.Net.Sockets;

int port = 8080;

var listener = new TcpListener(IPAddress.Any, port);
listener.Start();

Console.WriteLine($"BalanceX started on port {port}");

while (true)
{
    var client = await listener.AcceptTcpClientAsync();
    // Handle requests async    
    _ = Task.Run(() => RequestHandler.HandleRequest(client));
}

