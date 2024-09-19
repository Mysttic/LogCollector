public interface ILoggerCacheService
{
	Task<string?> TryGetStringAsync(string key);
	Task TrySetStringAsync(string key, string value);
	Task <TResult?> TryGetResultAsync<TResult>(string key);
	Task TrySetResultAsync<TResult>(string key, TResult value);
	bool CheckRedisAvailability();
	Task ClearCache(string key);
}

