using BalanceX.Servers; // Assuming Listener class is in this namespace
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
            Arguments = "/c start_servers.bat", // /c runs the batch script and exits
            WorkingDirectory = @"C:\Users\iandz\Desktop\Projects\BalanceX\Servers\", // Set the correct path to your batch file
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
