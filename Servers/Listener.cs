using BalanceX.Handlers;
using System.Net;
using System.Net.Sockets;

namespace BalanceX.Servers
{
    public class Listener
    {
        private int _port;

        public Listener(int port)
        {
            _port = port;
        }

        public async Task StartListening()
        {
            var listener = new TcpListener(IPAddress.Any, _port);
            listener.Start();

            Console.WriteLine($"BalanceX started on port {_port}");

            while (true)
            {
                var client = await listener.AcceptTcpClientAsync();
                _ = Task.Run(() => RequestHandler.HandleRequest(client, "Load Balancer"));
            }
        }
    }
}
