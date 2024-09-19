using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
	protected readonly LogCollectorDbContext _context;
	protected readonly IMapper _mapper;
	protected readonly ILoggerCacheService _cache;
	protected readonly string _cacheKey;

	public GenericRepository(LogCollectorDbContext context, IMapper mapper, ILoggerCacheService cache)
	{
		_context = context;
		_mapper = mapper;
		_cache = cache;
		_cacheKey = this.GetType().Name;
	}

	public async Task<TResult> AddAsync<TSource, TResult>(TSource entity)
	{
		T mappedEntity = _mapper.Map<T>(entity);
		await _context.AddAsync(mappedEntity);
		await _context.SaveChangesAsync();
		await _cache.ClearCache(this.GetType().Name);
		TResult? result = _mapper.Map<TResult>(mappedEntity);
		return result;
	}

	public async Task<T?> GetAsync(int? id)
	{
		T? result = await _cache.TryGetResultAsync<T?>($"{this.GetType().Name}-{id}");
		result ??= await _context.Set<T>().FindAsync(id);		
		await _cache.TrySetResultAsync($"{this.GetType().Name}-{id}", result);
		return result;
	}

	public async Task<TResult> GetAsync<TResult>(int? id)
	{
		T? result = await _cache.TryGetResultAsync<T?>($"{this.GetType().Name}-{id}");
		result ??= await _context.Set<T>().FindAsync(id);
		if (result is null)
			throw new NotFoundException(typeof(T).Name, id.HasValue ? id : "No Key Provided");
		return _mapper.Map<TResult>(result);
	}

	public async Task<PagedResult<TResult>> GetAllAsync<TResult>(IQueryParameters QueryParameters)
	{
		var totalSize = await _context.Set<T>().CountAsync();
		var items = await _context.Set<T>()
			.Skip(QueryParameters.StartIndex)
			.Take(QueryParameters.PageSize)
			.ProjectTo<TResult>(_mapper.ConfigurationProvider)
			.ToListAsync();

		return new PagedResult<TResult>
		{
			Items = items,
			PageNumber = QueryParameters.PageNumber,
			RecordNumber = QueryParameters.PageSize,
			TotalCount = totalSize
		};
	}

	public async Task UpdateAsync<TSource>(int id, TSource entity)
	{
		T? existingEntity = await GetAsync(id) ?? throw new NotFoundException(typeof(T).Name, id.ToString());
		T? mappedEntity = _mapper.Map(entity, existingEntity);
		_context.Update(mappedEntity);
		await _context.SaveChangesAsync();
		await _cache.ClearCache(this.GetType().Name);
		await _cache.TrySetResultAsync($"{this.GetType().Name}-{id}", mappedEntity);
	}

	public async Task DeleteAsync(int id)
	{
		T? entity = await GetAsync(id) ?? throw new NotFoundException(typeof(T).Name, id.ToString());
		_context.Set<T>().Remove(entity);
		await _context.SaveChangesAsync();
		await _cache.ClearCache(this.GetType().Name);
		await _cache.ClearCache($"{this.GetType().Name}-{id}");
	}

	public async Task<bool> ExistsAsync(int id)
	{
		T? entity = await GetAsync(id);
		return entity != null;
	}
}