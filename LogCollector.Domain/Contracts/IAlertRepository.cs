public interface IAlertRepository : IGenericRepository<Alert>
{
	Task<PagedResult<TResult>> GetAllAsync<TResult>(IAlertQueryParameters alertQueryParameters) where TResult : BaseAlertDto;

	Task<BaseAlertDto> GetAlertDetailsAsync(int id);
}