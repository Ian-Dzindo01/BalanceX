using System.Net;
using System.Net.Sockets;

int port = 8080;

var listener = new TcpListener(IPAddress.Any, port);
listener.Start();

Console.WriteLine($"Balancer started on port {port}");

