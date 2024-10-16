
public class Program
{
	public static async Task Main(string[] args)
	{
		Console.WriteLine("Starting Docker Manager...");

		var dockerManager = new DockerRedisManager();
		await dockerManager.StartRedisContainer();

		await dockerManager.MonitorRedisLogsAsync();

		Console.CancelKeyPress += async (sender, args) =>
		{
			Console.WriteLine("Shutting down Docker Manager (Ctrl+C detected)...");
			await dockerManager.StopRedisContainer();
			args.Cancel = true;
			Environment.Exit(0);
		};

		AppDomain.CurrentDomain.ProcessExit += async (sender, args) =>
		{
			Console.WriteLine("Shutting down Docker Manager (process exit detected)...");
			await dockerManager.StopRedisContainer();
		};
	}
}