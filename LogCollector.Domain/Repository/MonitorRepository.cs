using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

public class MonitorRepository : GenericRepository<Monitor>, IMonitorRepository
{
	public MonitorRepository(LogCollectorDbContext context, IMapper mapper, ILoggerCacheService cache) : base(context, mapper, cache)
	{
	}

	public async Task<PagedResult<TResult>> GetAllAsync<TResult>(IMonitorQueryParameters monitorQueryParameters) where TResult : BaseMonitorDto
	{
		List<TResult>? items = await _cache.TryGetResultAsync<List<TResult>>(_cacheKey);
		if (items == null)
		{
			items = await _context.Set<Monitor>()
				.Where(m => m.IsActive)
				.ProjectTo<TResult>(_mapper.ConfigurationProvider)
				.ToListAsync();

			await _cache.TrySetResultAsync(_cacheKey, items);
		}

		List<TResult> filteredItems = items
				.Where(m => string.IsNullOrEmpty(monitorQueryParameters.IsActive) ||
					(monitorQueryParameters.IsActive.ToLower() == "true" && m.IsActive) ||
					(monitorQueryParameters.IsActive.ToLower() == "false" && !m.IsActive))
				.Where(m => string.IsNullOrEmpty(monitorQueryParameters.Name) || m.Name!.ToLower().Contains(monitorQueryParameters.Name.ToLower()))
				.Where(m => string.IsNullOrEmpty(monitorQueryParameters.Action) || m.Action!.ToLower().Contains(monitorQueryParameters.Action.ToLower()))
				.Where(m => string.IsNullOrEmpty(monitorQueryParameters.Query) || m.Query!.ToLower().Contains(monitorQueryParameters.Query.ToLower()))
				.Skip(monitorQueryParameters.StartIndex)
				.Take(monitorQueryParameters.PageSize)
				.ToList();

		PagedResult<TResult> result = new PagedResult<TResult>
		{
			Items = filteredItems,
			PageNumber = monitorQueryParameters.PageNumber,
			RecordNumber = monitorQueryParameters.PageSize,
			TotalCount = items.Count
		};
		return result;
	}

	public async Task<BaseMonitorDto> GetMonitorDetailsAsync(int id)
	{
		BaseMonitorDto? cachedData = await _cache.TryGetResultAsync<BaseMonitorDto>($"{_cacheKey}-{id}");

		if (cachedData != null)
			return cachedData;

		BaseMonitorDto? monitor = await _context.Monitors
			.Where(m => m.IsActive)
			.Select(m => new BaseMonitorDto
			{
				Id = m.Id,
				Name = m.Name,
				Description = m.Description,
				IsActive = m.IsActive,
				LastInvoke = m.LastInvoke,
				Alerts = m.Alerts
			.OrderByDescending(a => a.CreatedAt)
			.Take(10)
			.Select(a => new BaseAlertDto
			{
				Message = a.Message,
				Content = a.Content,
				MonitorId = a.MonitorId,
				CreatedAt = a.CreatedAt,
				UpdatedAt = a.UpdatedAt,
				InvokedAt = a.InvokedAt,
				Id = a.Id
			})
			.ToList()
			})
			.FirstOrDefaultAsync(m => m.Id == id);

		if (monitor == null)
			throw new NotFoundException(nameof(Monitor), id);

		await _cache.TrySetResultAsync($"{_cacheKey}-{id}", monitor);

		return monitor;
	}
}