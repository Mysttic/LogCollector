using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

public class LogEntryRepository : GenericRepository<LogEntry>, ILogEntryRepository
{
	public LogEntryRepository(LogCollectorDbContext context, IMapper mapper, ILoggerCacheService cache) : base(context, mapper, cache)
	{
	}

	public async Task<PagedResult<TResult>> GetAllAsync<TResult>(ILogQueryParameters logQueryParameters)
	{
		string cacheKey = GenerateCacheKey(logQueryParameters);
		string? cachedData = await _cache.TryGetStringAsync(cacheKey);

		if (!string.IsNullOrEmpty(cachedData))
		{
			PagedResult<TResult>? cachedResult = JsonConvert.DeserializeObject<PagedResult<TResult>>(cachedData);
			return cachedResult ?? new PagedResult<TResult>();
		}

		var totalSize = await _context.Set<LogEntry>().CountAsync();
		var items = await _context.Set<LogEntry>()
			.Where(log => !string.IsNullOrEmpty(logQueryParameters.DeviceId) ? log.DeviceId!.ToLower().Contains(logQueryParameters.DeviceId.ToLower()) : true)
			.Where(log => !string.IsNullOrEmpty(logQueryParameters.ApplicationName) ? log.ApplicationName!.ToLower().Contains(logQueryParameters.ApplicationName.ToLower()) : true)
			.Where(log => !string.IsNullOrEmpty(logQueryParameters.IpAddress) ? log.IpAddress!.ToLower().Contains(logQueryParameters.IpAddress.ToLower()) : true)
			.Where(log => !string.IsNullOrEmpty(logQueryParameters.LogType) ? log.LogType!.ToLower().Contains(logQueryParameters.LogType.ToLower()) : true)
			.Where(log => !string.IsNullOrEmpty(logQueryParameters.LogMessage) ? log.LogMessage!.ToLower().Contains(logQueryParameters.LogMessage.ToLower()) : true)
			.Where(log => log.CreatedAt >= (logQueryParameters.StartDate.HasValue ? logQueryParameters.StartDate.Value.LocalDateTime : DateTime.MinValue)
						&& log.CreatedAt <= (logQueryParameters.EndDate.HasValue ? logQueryParameters.EndDate.Value.LocalDateTime.AddDays(1) : DateTime.MaxValue))
			.Skip(logQueryParameters.StartIndex)
			.Take(logQueryParameters.PageSize)
			.ProjectTo<TResult>(_mapper.ConfigurationProvider)
			.ToListAsync();

		PagedResult<TResult> result = new PagedResult<TResult>
		{
			Items = items,
			PageNumber = logQueryParameters.PageNumber,
			RecordNumber = logQueryParameters.PageSize,
			TotalCount = totalSize
		};

		string serializedResult = JsonConvert.SerializeObject(result);
		await _cache.TrySetStringAsync(cacheKey, serializedResult);

		return result;
	}

	private string GenerateCacheKey(ILogQueryParameters logQueryParameters)
	{
		// Użyj tylko daty z wartości StartDate i EndDate (jeśli są podane)
		var startDate = logQueryParameters.StartDate.HasValue ? logQueryParameters.StartDate.Value.Date.ToString("yyyy-MM-dd") : "NoStartDate";
		var endDate = logQueryParameters.EndDate.HasValue ? logQueryParameters.EndDate.Value.Date.ToString("yyyy-MM-dd") : "NoEndDate";

		// Generowanie klucza z wykorzystaniem DeviceId, ApplicationName, IpAddress itp.
		return $"{logQueryParameters.DeviceId}-{logQueryParameters.ApplicationName}-{logQueryParameters.IpAddress}-{logQueryParameters.LogType}-{startDate}-{endDate}-{logQueryParameters.PageNumber}-{logQueryParameters.PageSize}";
	}

}