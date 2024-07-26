public class LogCollectorMiddleware
{
	private readonly RequestDelegate _next;
	private const string APIKEYNAME = "ApiKey";

	public LogCollectorMiddleware(RequestDelegate next)
	{
		_next = next;
	}

	public async Task InvokeAsync(HttpContext context, IConfiguration configuration)
	{
		if (!context.Request.Headers.TryGetValue(APIKEYNAME, out var extractedApiKey))
		{
			context.Response.StatusCode = 401; // Unauthorized
			await context.Response.WriteAsync("API Key was not provided.");
			return;
		}

		var apiKey = configuration.GetValue<string>(APIKEYNAME);

		if (!apiKey.Equals(extractedApiKey))
		{
			context.Response.StatusCode = 401; // Unauthorized
			await context.Response.WriteAsync("Unauthorized client.");
			return;
		}

		await _next(context);
	}
}
