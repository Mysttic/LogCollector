﻿public interface IGenericRepository<T> where T : class
{
	Task<T> GetAsync(int? id);

	Task<TResult> GetAsync<TResult>(int? id);

	Task<List<T>> GetAllAsync();

	Task<List<TResult>> GetAllAsync<TResult>();

	Task<PagedResult<TResult>> GetAllAsync<TResult>(IQueryParameters QueryParameters);

	Task<T> AddAsync(T entity);

	Task<TResult> AddAsync<TSource, TResult>(TSource entity);

	Task DeleteAsync(int id);

	Task UpdateAsync(T entity);

	Task UpdateAsync<TSource>(int id, TSource entity);

	Task<bool> ExistsAsync(int id);

	string GenerateCacheKey(IQueryParameters queryParameters);
}