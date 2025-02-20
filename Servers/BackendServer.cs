using BalanceX.Handlers;
using System.Net;
using System.Net.Sockets;

namespace BalanceX.Servers
{
    class BackendServer
    {
        private int _port;

        public BackendServer(int port)
        {
            _port = port;
        }

        public async Task Run()
        {
            var listener = new TcpListener(IPAddress.Any, _port);
            listener.Start();
            Console.WriteLine($"Backend Server started on port {_port}...");

            while (true)
            {
                var client = await listener.AcceptTcpClientAsync();
                _ = Task.Run(() => RequestHandler.HandleRequest(client, "Backend Server"));
            }
        }
    }
}
