using System.Net;
using System.Net.Sockets;
using System.Text;

namespace BalanceX.Servers
{
    class BackendServer
    {
        private readonly int _port;

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
                _ = Task.Run(() => HandleRequest(client));
            }
        }

        private static async Task HandleRequest(TcpClient client)
        {
            using var stream = client.GetStream();
            var buffer = new byte[1024];
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            string request = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            Console.WriteLine($"Received request from {client.Client.RemoteEndPoint}:\n{request}");

            string response = $"HTTP/1.1 200 OK\r\nContent-Length: 25\r\n\r\nHello from Backend Server!";

            byte[] responseBytes = Encoding.UTF8.GetBytes(response);

            Console.WriteLine($"Sending response:\n{response}");

            await stream.WriteAsync(responseBytes, 0, responseBytes.Length);

            client.Close();
        }
    }
}
