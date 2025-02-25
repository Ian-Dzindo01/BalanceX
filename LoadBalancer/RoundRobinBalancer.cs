namespace BalanceX.LoadBalancer
{
    internal class RoundRobinBalancer
    {
        private readonly List<int> _servers = new() { 8080, 8081, 8082 };
        private int _currentIndex = -1;
        private readonly object _lock = new();

        public int GetNextServer()
        {
            // One thread at a time
            lock (_lock)
            {
                // Goes back to the beginning
                _currentIndex = (_currentIndex + 1) % _servers.Count;
                return _servers[_currentIndex];
            }
        }
    }
}