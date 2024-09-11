using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

public class LogEntryRepository : GenericRepository<LogEntry>, ILogEntryRepository
{
	public LogEntryRepository(LogCollectorDbContext context, IMapper mapper) : base(context, mapper)
	{
	}

	public async Task<PagedResult<TResult>> GetAllAsync<TResult>(ILogQueryParameters logQueryParameters)
	{
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

		return new PagedResult<TResult>
		{
			Items = items,
			PageNumber = logQueryParameters.PageNumber,
			RecordNumber = logQueryParameters.PageSize,
			TotalCount = totalSize
		};
	}
}