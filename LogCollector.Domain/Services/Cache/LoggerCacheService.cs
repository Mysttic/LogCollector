﻿using Microsoft.Extensions.Caching.Distributed;

public class LoggerCacheService : ILoggerCacheService
{
	private readonly IDistributedCache _cache;
	private bool _isRedisAvailable;

	public LoggerCacheService(IDistributedCache cache)
	{
		_cache = cache;
		_isRedisAvailable = CheckRedisAvailability();
	}

	public bool CheckRedisAvailability()
	{
		try
		{
			_cache.SetString("TestKey", "TestValue");
			_cache.Remove("TestKey");
			return true;
		}
		catch
		{
			return false;
		}
	}

	public async Task<string?> TryGetStringAsync(string cacheKey)
	{

		if (!_isRedisAvailable)
		{
			return null;
		}

		try
		{
			return await _cache.GetStringAsync(cacheKey);
		}
		catch
		{
			_isRedisAvailable = false;
			return null;
		}
	}

	public async Task TrySetStringAsync(string cacheKey, string value)
	{
		if (!_isRedisAvailable)
		{
			return;
		}

		try
		{
			await _cache.SetStringAsync(cacheKey, value, new DistributedCacheEntryOptions
			{
				AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
			});
		}
		catch
		{
			_isRedisAvailable = false;
		}
	}

}
