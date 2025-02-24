using System.Net.Sockets;
using System.Text;


namespace BalanceX.Handlers
{
    public class RequestHandler
    {
        public static async Task HandleRequest(TcpClient client)
        {
            using var clientStream = client.GetStream();
            var buffer = new byte[1024];
            int bytesRead = await clientStream.ReadAsync(buffer, 0, buffer.Length);
            string request = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            Console.WriteLine("Hello from Listener. \n");
            Console.WriteLine($"Received request from {client.Client.RemoteEndPoint}:\n{request}");
            Console.WriteLine("Forwarding to backend server... \n");

            var backendResponse = await ForwardRequestToBackend();

            byte[] responseBytes = Encoding.UTF8.GetBytes(backendResponse);
            await clientStream.WriteAsync(responseBytes, 0, responseBytes.Length);

            client.Close();
        }

        public static async Task<string> ForwardRequestToBackend()
        {
            string backendAddress = "127.0.0.1";
            int backendPort = 8085;

            using var backendClient = new TcpClient(backendAddress, backendPort);
            using var stream = backendClient.GetStream();

            string request = "GET / HTTP/1.1\r\nHost: localhost\r\n\r\n";
            byte[] requestBytes = Encoding.UTF8.GetBytes(request);
            await stream.WriteAsync(requestBytes, 0, requestBytes.Length);

            var buffer = new byte[1024];
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer, 0, bytesRead);
        }
    }
}
