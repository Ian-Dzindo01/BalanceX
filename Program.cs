using BalanceX.Servers;
using System.Diagnostics;


try
{
    StartBatchScript();

    var listener = new Listener(8076);
    await listener.StartListening();

    Console.WriteLine("Listener started successfully on port 8076.");

}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}

static void StartBatchScript()
{
    try
    {
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = "cmd.exe",
            Arguments = "/c start_servers.bat",
            WorkingDirectory = @"C:\Users\iandz\Desktop\Projects\BalanceX\Servers\",
            UseShellExecute = true
        };

        Process.Start(startInfo);
        Console.WriteLine("Batch script executed successfully!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error starting batch script: {ex.Message}");
    }
}