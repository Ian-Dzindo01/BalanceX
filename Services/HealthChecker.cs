using BalanceX.Handlers;

namespace BalanceX.Services
{
    public class HealthChecker
    {
        private readonly HttpClient _httpClient;

        public HealthChecker()
        {
            _httpClient = new HttpClient(new HttpClientHandler
            {
                MaxConnectionsPerServer = 10,
            })
            {
                Timeout = TimeSpan.FromSeconds(5)
            };
        }

        public async Task<bool> CheckHealth(int port)
        {
            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, $"http://localhost:{port}/health");
                request.Headers.Connection.Clear();
                request.Headers.Connection.Add("keep-alive");

                request.Headers.Add("X-API-KEY", KeyVaultHelper.GetApiKey());

                using HttpResponseMessage response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

                Console.WriteLine($"Health check response from port {port}: {response.StatusCode}");
                string content = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Response Content: {content}");

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while checking health of port {port}: {ex.Message}");
                return false;
            }
        }
    }
}
