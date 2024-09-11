using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

public class MonitorRepository : GenericRepository<Monitor>, IMonitorRepository
{
	public MonitorRepository(LogCollectorDbContext context, IMapper mapper) : base(context, mapper)
	{
	}

	public async Task<PagedResult<TResult>> GetAllAsync<TResult>(IMonitorQueryParameters monitorQueryParameters)
	{
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

		return new PagedResult<TResult>
		{
			Items = items,
			PageNumber = monitorQueryParameters.PageNumber,
			RecordNumber = monitorQueryParameters.PageSize,
			TotalCount = totalSize
		};
	}

	public async Task<BaseMonitorDto> GetMonitorDetailsAsync(int id)
	{
		var monitor = await _context.Monitors
			.Where(m => m.IsActive)
			.Include(m => m.Alerts)
			.ProjectTo<BaseMonitorDto>(_mapper.ConfigurationProvider)
			.FirstOrDefaultAsync(m => m.Id == id);

		if (monitor == null)
		{
			throw new NotFoundException(nameof(Monitor), id);
		}

		return monitor;
	}
}