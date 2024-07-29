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
			.Where(log => !string.IsNullOrEmpty(logQueryParameters.DeviceId) ? log.DeviceId == logQueryParameters.DeviceId : true)
			.Where(log => !string.IsNullOrEmpty(logQueryParameters.ApplicationName) ? log.ApplicationName == logQueryParameters.ApplicationName : true)
			.Where(log => !string.IsNullOrEmpty(logQueryParameters.IpAddress) ? log.IpAddress == logQueryParameters.IpAddress : true)
			.Where(log => !string.IsNullOrEmpty(logQueryParameters.LogMessage) ? log.LogMessage == logQueryParameters.LogMessage : true)
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

