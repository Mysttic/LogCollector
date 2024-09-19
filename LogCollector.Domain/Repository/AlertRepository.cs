using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

public class AlertRepository : GenericRepository<Alert>, IAlertRepository
{
	public AlertRepository(LogCollectorDbContext context, IMapper mapper, ILoggerCacheService cache) : base(context, mapper, cache)
	{
	}

	public async Task<PagedResult<TResult>> GetAllAsync<TResult>(IAlertQueryParameters alertQueryParameters) where TResult : BaseAlertDto
	{
		List<TResult>? items = await _cache.TryGetResultAsync<List<TResult>>(_cacheKey);
		if (items == null)
		{
			items = await _context.Set<Alert>()
				.ProjectTo<TResult>(_mapper.ConfigurationProvider)
				.ToListAsync();

			await _cache.TrySetResultAsync(_cacheKey, items);
		}

		List<TResult> filteredItems = items
				.Where(a => alertQueryParameters.MonitorId > 0 ? a.MonitorId == alertQueryParameters.MonitorId : true)
				.Where(a => !string.IsNullOrEmpty(alertQueryParameters.Message) ? a.Message!.ToLower().Contains(alertQueryParameters.Message.ToLower()) : true)
				.Where(a => !string.IsNullOrEmpty(alertQueryParameters.Content) ? a.Content!.ToLower().Contains(alertQueryParameters.Content.ToLower()) : true)
				.Skip(alertQueryParameters.StartIndex)
				.Take(alertQueryParameters.PageSize)
				.ToList();

		PagedResult<TResult> result = new PagedResult<TResult>
		{
			Items = filteredItems,
			PageNumber = alertQueryParameters.PageNumber,
			RecordNumber = alertQueryParameters.PageSize,
			TotalCount = items.Count
		};
		return result;
	}

	public async Task<BaseAlertDto> GetAlertDetailsAsync(int id)
	{
		BaseAlertDto? cachedData = await _cache.TryGetResultAsync<BaseAlertDto>($"{_cacheKey}-{id}");

		if (cachedData != null)
			return cachedData;

		BaseAlertDto? alert = await _context.Set<Alert>()
			.Where(a => a.Id == id)
			.ProjectTo<BaseAlertDto>(_mapper.ConfigurationProvider)
			.FirstOrDefaultAsync();

		if (alert == null)
			throw new NotFoundException(nameof(Alert), id);

		await _cache.TrySetResultAsync($"{_cacheKey}-{id}", alert);

		return alert;
	}

}