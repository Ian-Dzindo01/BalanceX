namespace BalanceX.LoadBalancer
{
    public class RoundRobinBalancer
    {
        private readonly List<int> _servers = new() { 8080, 8081, 8082 };
        private readonly Dictionary<int, bool> _serverHealth = new();
        private int _currentIndex = -1;
        private readonly object _lock = new();
        private readonly HttpClient _httpClient;

        public RoundRobinBalancer()
        {
            foreach (var server in _servers)
            {
                _serverHealth[server] = true;
            }

            _httpClient = new HttpClient(new HttpClientHandler
            {
                MaxConnectionsPerServer = 10,
            })
            {
                Timeout = TimeSpan.FromSeconds(5)
            };

            Task.Run(() => StartHealthCheck());
        }

        public int GetNextServer()
        {
            lock (_lock)
            {
                var healthyServers = _servers.FindAll(port => _serverHealth.ContainsKey(port) && _serverHealth[port]);

                if (healthyServers.Count == 0)
                {
                    throw new Exception("No healthy servers available");
                }

                _currentIndex = (_currentIndex + 1) % healthyServers.Count;
                return healthyServers[_currentIndex];
            }
        }

        private async Task StartHealthCheck()
        {
            while (true)
            {
                foreach (var server in _servers)
                {
                    bool isHealthy = await CheckHealth(server);
                    _serverHealth[server] = isHealthy;
                }

                Console.WriteLine("Health Check Status: " + string.Join(", ", _serverHealth.Select(kvp => $"{kvp.Key}: {(kvp.Value ? "Healthy" : "Unhealthy")}")));
                await Task.Delay(10000);
            }
        }

        private async Task<bool> CheckHealth(int port)
        {
            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, $"http://localhost:{port}/health");

                request.Headers.Connection.Clear();
                request.Headers.Connection.Add("keep-alive");

                HttpResponseMessage response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

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