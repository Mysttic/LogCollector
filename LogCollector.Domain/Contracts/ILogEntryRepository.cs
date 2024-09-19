public interface ILogEntryRepository : IGenericRepository<LogEntry>
{
	Task<PagedResult<TResult>> GetAllAsync<TResult>(ILogQueryParameters QueryParameters) where TResult : BaseLogEntryDto;

	Task<BaseLogEntryDto> GetLogDetailsAsync(int id);
}