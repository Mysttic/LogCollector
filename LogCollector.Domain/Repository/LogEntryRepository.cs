using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

public class LogEntryRepository : GenericRepository<LogEntry>, ILogEntryRepository
{
	public LogEntryRepository(LogCollectorDbContext context, IMapper mapper) : base(context, mapper)
	{
		
	}

	public override async Task<PagedResult<TResult>> GetAllAsync<TResult>(QueryParameters queryParameters)
	{
		var totalSize = await _context.Set<LogEntry>().CountAsync();
		var items = await _context.Set<LogEntry>()
			.Where(log => !string.IsNullOrEmpty(queryParameters.DeviceId) ? log.DeviceId == queryParameters.DeviceId : true)
			.Where(log => !string.IsNullOrEmpty(queryParameters.ApplicationName) ? log.ApplicationName == queryParameters.ApplicationName : true)
			.Where(log => !string.IsNullOrEmpty(queryParameters.IpAddress) ? log.IpAddress == queryParameters.IpAddress : true)
			.Where(log => !string.IsNullOrEmpty(queryParameters.LogMessage) ? log.LogMessage == queryParameters.LogMessage : true)
			.Skip(queryParameters.StartIndex)
			.Take(queryParameters.PageSize)
			.ProjectTo<TResult>(_mapper.ConfigurationProvider)
			.ToListAsync();

		return new PagedResult<TResult>
		{
			Items = items,
			PageNumber = queryParameters.PageNumber,
			RecordNumber = queryParameters.PageSize,
			TotalCount = totalSize
		};
	}
}

