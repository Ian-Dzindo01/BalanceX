using BalanceX.LoadBalancer;
using System.Net.Sockets;
using System.Text;

namespace BalanceX.Handlers
{
    public static class RequestHandler
    {
        private static readonly RoundRobinBalancer _balancer = new();
        private static readonly HttpClient _httpClient = new();

        public static async Task HandleRequest(TcpClient client)
        {
            using var clientStream = client.GetStream();
            var buffer = new byte[1024];
            int bytesRead = await clientStream.ReadAsync(buffer, 0, buffer.Length);
            string request = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            Console.WriteLine("Hello from Listener.\n");
            Console.WriteLine($"Received request from {client.Client.RemoteEndPoint}:\n{request}");

            // Use Round Robin to select the backend server
            int backendPort = _balancer.GetNextServer();
            Console.WriteLine($"Forwarding request to backend server on port {backendPort}...\n");

            var backendResponse = await ForwardRequestToBackend(backendPort);

            byte[] responseBytes = Encoding.UTF8.GetBytes(backendResponse);
            await clientStream.WriteAsync(responseBytes, 0, responseBytes.Length);

            client.Close();
        }

        public static async Task<string> ForwardRequestToBackend(int backendPort)
        {
            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, $"http://localhost:{backendPort}/");
                request.Headers.Connection.Clear();
                request.Headers.Connection.Add("keep-alive");

                HttpResponseMessage response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                string responseBody = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Forwarded request to {backendPort}, Response: {response.StatusCode} - {responseBody}");

                return responseBody;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error forwarding request to {backendPort}: {ex.Message}");
                return "Backend Error";
            }
        }
    }
}
