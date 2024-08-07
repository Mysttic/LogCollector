public interface ILogEntryRepository : IGenericRepository<LogEntry>
{
	Task<PagedResult<TResult>> GetAllAsync<TResult>(ILogQueryParameters QueryParameters);

	Task<BaseLogEntryDto> GetLogDetailsAsync(int id);
}
