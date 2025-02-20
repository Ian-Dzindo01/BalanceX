using System.Net.Sockets;
using System.Text;


namespace BalanceX.Handlers
{
    public class RequestHandler
    {
        public static async Task HandleRequest(TcpClient client, string message)
        {
            using var stream = client.GetStream();
            var buffer = new byte[1024];
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            string request = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            Console.WriteLine($"Received request from {client.Client.RemoteEndPoint}:\n{request}");

            string response = $"HTTP/1.1 200 OK\r\nContent-Length: 25\r\n\r\nHello from {message}!";

            byte[] responseBytes = Encoding.UTF8.GetBytes(response);

            await stream.WriteAsync(responseBytes, 0, responseBytes.Length);

            client.Close();
        }
    }
}
