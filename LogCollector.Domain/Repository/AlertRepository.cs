using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

public class AlertRepository : GenericRepository<Alert>, IAlertRepository
{
	public AlertRepository(LogCollectorDbContext context, IMapper mapper, ILoggerCacheService cache) : base(context, mapper, cache)
	{
	}

	public async Task<PagedResult<TResult>> GetAllAsync<TResult>(IAlertQueryParameters alertQueryParameters)
	{
		string cacheKey = GenerateCacheKey(alertQueryParameters);
		string? cachedData = await _cache.TryGetStringAsync(cacheKey);

		if (!string.IsNullOrEmpty(cachedData))
		{
			PagedResult<TResult>? cachedResult = JsonConvert.DeserializeObject<PagedResult<TResult>>(cachedData);
			return cachedResult ?? new PagedResult<TResult>();
		}

		var totalSize = await _context.Set<Alert>().CountAsync();
		var items = await _context.Set<Alert>()
			.Where(a => alertQueryParameters.MonitorId > 0 ? a.MonitorId == alertQueryParameters.MonitorId : true)
			.Where(a => !string.IsNullOrEmpty(alertQueryParameters.Message) ? a.Message!.ToLower().Contains(alertQueryParameters.Message.ToLower()) : true)
			.Where(a => !string.IsNullOrEmpty(alertQueryParameters.Content) ? a.Content!.ToLower().Contains(alertQueryParameters.Content.ToLower()) : true)
			.Skip(alertQueryParameters.StartIndex)
			.Take(alertQueryParameters.PageSize)
			.ProjectTo<TResult>(_mapper.ConfigurationProvider)
			.ToListAsync();

		PagedResult<TResult> result = new PagedResult<TResult>
		{
			Items = items,
			PageNumber = alertQueryParameters.PageNumber,
			RecordNumber = alertQueryParameters.PageSize,
			TotalCount = totalSize
		};

		string serializedResult = JsonConvert.SerializeObject(result);
		await _cache.TrySetStringAsync(cacheKey, serializedResult);

		return result;
	}

	public async Task<BaseAlertDto> GetAlertDetailsAsync(int id)
	{
		string cacheKey = $"Alert-{id}";
		string? cachedData = await _cache.TryGetStringAsync(cacheKey);

		if (!string.IsNullOrEmpty(cachedData))
		{
			BaseAlertDto? cachedResult = JsonConvert.DeserializeObject<BaseAlertDto>(cachedData);
			return cachedResult ?? new BaseAlertDto();
		}

		var alert = await _context.Set<Alert>()
			.Where(a => a.Id == id)
			.ProjectTo<BaseAlertDto>(_mapper.ConfigurationProvider)
			.FirstOrDefaultAsync();

		if (alert == null)
		{
			throw new NotFoundException(nameof(Alert), id);
		}

		string serializedResult = JsonConvert.SerializeObject(alert);
		await _cache.TrySetStringAsync(cacheKey, serializedResult);

		return alert;
	}

	private string GenerateCacheKey(IAlertQueryParameters alertQueryParameters)
	{
		// Generowanie klucza z wykorzystaniem MonitorId, Message, Content itp.
		return $"{alertQueryParameters.MonitorId}-{alertQueryParameters.Message}-{alertQueryParameters.Content}-{alertQueryParameters.PageNumber}-{alertQueryParameters.PageSize}";
	}
}