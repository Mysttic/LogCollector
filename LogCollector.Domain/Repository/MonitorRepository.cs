using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

public class MonitorRepository : GenericRepository<Monitor>, IMonitorRepository
{
	public MonitorRepository(LogCollectorDbContext context, IMapper mapper, IDistributedCache cache) : base(context, mapper, cache)
	{
	}

	public async Task<PagedResult<TResult>> GetAllAsync<TResult>(IMonitorQueryParameters monitorQueryParameters)
	{
		string cacheKey = GenerateCacheKey(monitorQueryParameters);
		string? cachedData = await _cache.GetStringAsync(cacheKey);

		if (!string.IsNullOrEmpty(cachedData))
		{
			PagedResult<TResult>? cachedResult = JsonConvert.DeserializeObject<PagedResult<TResult>>(cachedData);
			return cachedResult ?? new PagedResult<TResult>();
		}

		var totalSize = await _context.Set<Monitor>().Where(m => m.IsActive).CountAsync();
		var items = await _context.Set<Monitor>()
			.Where(m => !string.IsNullOrEmpty(monitorQueryParameters.IsActive)
				?
					monitorQueryParameters.IsActive.ToLower() == "true"
						?
							m.IsActive
						:
							monitorQueryParameters.IsActive.ToLower() == "false"
							?
								!m.IsActive
							:
							true
				:
					true)
			.Where(m => !string.IsNullOrEmpty(monitorQueryParameters.Name) ? m.Name!.ToLower().Contains(monitorQueryParameters.Name.ToLower()) : true)
			.Where(m => !string.IsNullOrEmpty(monitorQueryParameters.Action) ? m.Action!.ToLower().Contains(monitorQueryParameters.Action.ToLower()) : true)
			.Where(m => !string.IsNullOrEmpty(monitorQueryParameters.Query) ? m.Query!.ToLower().Contains(monitorQueryParameters.Query.ToLower()) : true)
			.Skip(monitorQueryParameters.StartIndex)
			.Take(monitorQueryParameters.PageSize)
			.ProjectTo<TResult>(_mapper.ConfigurationProvider)
			.ToListAsync();

		PagedResult<TResult> result = new PagedResult<TResult>
		{
			Items = items,
			PageNumber = monitorQueryParameters.PageNumber,
			RecordNumber = monitorQueryParameters.PageSize,
			TotalCount = totalSize
		};

		string serializedResult = JsonConvert.SerializeObject(result);
		await _cache.SetStringAsync(cacheKey, serializedResult, new DistributedCacheEntryOptions
		{
			AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
		});

		return result;
	}

	public async Task<BaseMonitorDto> GetMonitorDetailsAsync(int id)
	{
		string cacheKey = $"MonitorDetails-{id}";
		string? cachedData = await _cache.GetStringAsync(cacheKey);

		if (!string.IsNullOrEmpty(cachedData))
		{
			BaseMonitorDto? cachedResult = JsonConvert.DeserializeObject<BaseMonitorDto>(cachedData);
			return cachedResult ?? new BaseMonitorDto();
		}

		var monitor = await _context.Monitors
			.Where(m => m.IsActive)
			.Include(m => m.Alerts)
			.ProjectTo<BaseMonitorDto>(_mapper.ConfigurationProvider)
			.FirstOrDefaultAsync(m => m.Id == id);

		if (monitor == null)
		{
			throw new NotFoundException(nameof(Monitor), id);
		}

		string serializedResult = JsonConvert.SerializeObject(monitor);
		await _cache.SetStringAsync(cacheKey, serializedResult, new DistributedCacheEntryOptions
		{
			AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
		});

		return monitor;
	}

	private string GenerateCacheKey(IMonitorQueryParameters monitorQueryParameters)
	{
		// Generowanie klucza z wykorzystaniem IsActive, Name, Action, Query itp.
		return $"{monitorQueryParameters.IsActive}-{monitorQueryParameters.Name}-{monitorQueryParameters.Action}-{monitorQueryParameters.Query}-{monitorQueryParameters.PageNumber}-{monitorQueryParameters.PageSize}";
	}
}