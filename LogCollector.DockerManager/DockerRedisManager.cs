using Docker.DotNet;
using Docker.DotNet.Models;
using System.Text;

public class DockerRedisManager
{
	private readonly DockerClient _dockerClient;

	public DockerRedisManager()
	{
		_dockerClient = new DockerClientConfiguration(new Uri("npipe://./pipe/docker_engine")).CreateClient();
	}
	public async Task StartRedisContainer()
	{
		var containers = await _dockerClient.Containers.ListContainersAsync(new ContainersListParameters() { All = true });
		var redisContainer = containers.FirstOrDefault(c => c.Names.Contains("/redis"));

		if (redisContainer == null)
		{
			var response = await _dockerClient.Containers.CreateContainerAsync(new CreateContainerParameters
			{
				Image = "redis",
				Name = "redis",
				ExposedPorts = new Dictionary<string, EmptyStruct>
				{
					{ "6379", new EmptyStruct() }
				},
				HostConfig = new HostConfig
				{
					PortBindings = new Dictionary<string, IList<PortBinding>>
					{
						{ "6379", new List<PortBinding> { new PortBinding { HostPort = "6379" } } }
					}
				}
			});

			await _dockerClient.Containers.StartContainerAsync(response.ID, new ContainerStartParameters());
		}
		else
		{
			// Jeśli kontener istnieje, sprawdź, czy działa
			if (redisContainer.State != "running")
			{
				await _dockerClient.Containers.StartContainerAsync(redisContainer.ID, new ContainerStartParameters());
			}
			else
			{
				Console.WriteLine("Kontener Redis już działa.");
			}
		}
	}

	public async Task StopRedisContainer()
	{
		var containers = await _dockerClient.Containers.ListContainersAsync(new ContainersListParameters());
		var redisContainer = containers.FirstOrDefault(c => c.Names.Contains("/redis"));

		if (redisContainer != null)
		{
			await _dockerClient.Containers.StopContainerAsync(redisContainer.ID, new ContainerStopParameters());
			await _dockerClient.Containers.RemoveContainerAsync(redisContainer.ID, new ContainerRemoveParameters());
		}
	}

	public async Task MonitorRedisLogsAsync()
	{
		var containers = await _dockerClient.Containers.ListContainersAsync(new ContainersListParameters());
		var redisContainer = containers.FirstOrDefault(c => c.Names.Contains("/redis"));

		if (redisContainer != null)
		{
			MultiplexedStream logs = await _dockerClient.Containers.GetContainerLogsAsync(
				redisContainer.ID,
				true,
				new ContainerLogsParameters
				{
					Follow = true,
					ShowStdout = true,
					ShowStderr = true
				});

			// Przetwarzanie logów
			var buffer = new byte[8192];
			while (true)
			{
				var result = await logs.ReadOutputAsync(buffer, 0, buffer.Length, default);
				if (result.EOF)
				{
					break;
				}

				var logOutput = Encoding.UTF8.GetString(buffer, 0, result.Count);
				Console.WriteLine(logOutput);  // Wypisz logi na konsolę
			}
		}
		else
		{
			Console.WriteLine("Redis container is not running.");
		}
	}

	public async Task<string> GetContainerStatusAsync()
	{
		var containers = await _dockerClient.Containers.ListContainersAsync(new ContainersListParameters());
		var redisContainer = containers.FirstOrDefault(c => c.Names.Contains("/redis"));

		if (redisContainer != null)
		{
			return redisContainer.State;
		}

		return "Container not found";
	}
}
