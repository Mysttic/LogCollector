public interface IGenericRepository<T> where T : class
{
	Task<T?> GetAsync(int? id);

	Task<TResult> GetAsync<TResult>(int? id);

	Task<PagedResult<TResult>> GetAllAsync<TResult>(IQueryParameters QueryParameters);

	Task<TResult> AddAsync<TSource, TResult>(TSource entity);

	Task DeleteAsync(int id);

	Task UpdateAsync<TSource>(int id, TSource entity);

	Task<bool> ExistsAsync(int id);

}