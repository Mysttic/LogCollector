public interface ILoggerCacheService
{
	Task<string?> TryGetStringAsync(string key);
	Task TrySetStringAsync(string key, string value);
	bool CheckRedisAvailability();
}

