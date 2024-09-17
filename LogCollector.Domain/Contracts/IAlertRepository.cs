public interface IAlertRepository : IGenericRepository<Alert>
{
	Task<PagedResult<TResult>> GetAllAsync<TResult>(IAlertQueryParameters alertQueryParameters);

	Task<BaseAlertDto> GetAlertDetailsAsync(int id);
}