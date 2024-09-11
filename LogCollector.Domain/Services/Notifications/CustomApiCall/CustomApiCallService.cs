using Microsoft.Extensions.Configuration;
using System.Text;

public class CustomApiCallService : ICustomApiCallService
{
	private readonly IConfiguration _configuration;
	private readonly HttpClient _httpClient;

	public CustomApiCallService(IConfiguration configuration, HttpClient httpClient)
	{
		_configuration = configuration;
		_httpClient = httpClient;
	}

	public async Task SendCustomApiCallAsync(string? url, string? authKey, string message)
	{
		Console.WriteLine($"Sending custom api call to {url} with authKey {authKey} and message {message}");

		if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(authKey))
		{
			throw new ArgumentException("URL and AuthKey must be provided");
		}

		var request = new HttpRequestMessage(HttpMethod.Post, url);
		request.Headers.Add("Authorization", $"Bearer {authKey}");
		request.Content = new StringContent(message, Encoding.UTF8, "application/json");

		var response = await _httpClient.SendAsync(request);

		if (!response.IsSuccessStatusCode)
		{
			var responseContent = await response.Content.ReadAsStringAsync();
			throw new Exception($"Failed to send custom API call. Status code: {response.StatusCode}, Response: {responseContent}");
		}

		Console.WriteLine("Custom API call sent successfully.");
	}
}