using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

public class LogEntryRepository : GenericRepository<LogEntry>, ILogEntryRepository
{
	public LogEntryRepository(LogCollectorDbContext context, IMapper mapper, ILoggerCacheService cache) : base(context, mapper, cache)
	{
	}

	public async Task<PagedResult<TResult>> GetAllAsync<TResult>(ILogQueryParameters logQueryParameters) where TResult : BaseLogEntryDto
	{
		List<TResult>? items = await _cache.TryGetResultAsync<List<TResult>>(_cacheKey);
		if (items == null)
		{
			items = await _context.Set<LogEntry>()
				.ProjectTo<TResult>(_mapper.ConfigurationProvider)
				.ToListAsync();

			await _cache.TrySetResultAsync(_cacheKey, items);
		}

		List<TResult> filteredItems = items
				.Where(log => string.IsNullOrEmpty(logQueryParameters.DeviceId) || log.DeviceId!.ToLower().Contains(logQueryParameters.DeviceId.ToLower()))
				.Where(log => string.IsNullOrEmpty(logQueryParameters.ApplicationName) || log.ApplicationName!.ToLower().Contains(logQueryParameters.ApplicationName.ToLower()))
				.Where(log => string.IsNullOrEmpty(logQueryParameters.IpAddress) || log.IpAddress!.ToLower().Contains(logQueryParameters.IpAddress.ToLower()))
				.Where(log => string.IsNullOrEmpty(logQueryParameters.LogType) || log.LogType!.ToLower().Contains(logQueryParameters.LogType.ToLower()))
				.Where(log => string.IsNullOrEmpty(logQueryParameters.LogMessage) || log.LogMessage!.ToLower().Contains(logQueryParameters.LogMessage.ToLower()))
				.Where(log => log.CreatedAt >= (logQueryParameters.StartDate.HasValue ? logQueryParameters.StartDate.Value.LocalDateTime : DateTime.MinValue)
							&& log.CreatedAt <= (logQueryParameters.EndDate.HasValue ? logQueryParameters.EndDate.Value.LocalDateTime.AddDays(1) : DateTime.MaxValue))
				.Skip(logQueryParameters.StartIndex)
				.Take(logQueryParameters.PageSize)
				.ToList();

		PagedResult<TResult> result = new PagedResult<TResult>
		{
			Items = filteredItems,
			PageNumber = logQueryParameters.PageNumber,
			RecordNumber = logQueryParameters.PageSize,
			TotalCount = items.Count
		};

		return result;

	}

	public async Task<BaseLogEntryDto> GetLogDetailsAsync(int id)
	{
		BaseLogEntryDto? cachedData = await _cache.TryGetResultAsync<BaseLogEntryDto>($"{_cacheKey}-{id}");

		if (cachedData != null)
			return cachedData;

		BaseLogEntryDto? log = await _context.Logs
			.ProjectTo<BaseLogEntryDto>(_mapper.ConfigurationProvider)
			.FirstOrDefaultAsync(l => l.Id == id);

		if (log == null)
			throw new NotFoundException(nameof(LogEntry), id);

		await _cache.TrySetResultAsync($"{_cacheKey}-{id}", log);

		return log;
	}
}