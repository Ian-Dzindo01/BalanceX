using BalanceX.Servers;

var listener = new Listener(8080);
var listenerTask = listener.StartListening();

var mockServer = new BackendServer(8085);
var mockServerTask = mockServer.Run();

await Task.WhenAll(listenerTask, mockServerTask);