public interface IMonitorRepository : IGenericRepository<Monitor>
{
	Task<PagedResult<TResult>> GetAllAsync<TResult>(IMonitorQueryParameters QueryParameters);

	Task<MonitorDto> GetMonitorDetailsAsync(int id);

	new Task DeleteAsync(int id);

}
