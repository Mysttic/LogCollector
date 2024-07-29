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
			.Where(m => m.IsActive)
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
			.Where(m=>m.IsActive)
			.Include(m => m.Alerts)
			.ProjectTo<BaseMonitorDto>(_mapper.ConfigurationProvider)
			.FirstOrDefaultAsync(m => m.Id == id);

		if (monitor == null)
		{
			throw new NotFoundException(nameof(Monitor), id);
		}

		return monitor;
	}

	public async new Task DeleteAsync(int id)
	{
		var monitor = await _context.Monitors.FindAsync(id);
		if (monitor == null)
		{
			throw new NotFoundException(nameof(Monitor), id);
		}
		monitor.IsActive = false;
		_context.Monitors.Update(monitor);
		await _context.SaveChangesAsync();
	}
}