public interface IMonitorRepository : IGenericRepository<Monitor>
{
	Task<PagedResult<TResult>> GetAllAsync<TResult>(IMonitorQueryParameters QueryParameters) where TResult : BaseMonitorDto;

	Task<BaseMonitorDto> GetMonitorDetailsAsync(int id);
}