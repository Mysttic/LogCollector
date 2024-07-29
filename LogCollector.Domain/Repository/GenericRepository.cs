using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
	protected readonly LogCollectorDbContext _context;
	protected readonly IMapper _mapper;

	public GenericRepository(LogCollectorDbContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public async Task<T> AddAsync(T entity)
	{
		await _context.AddAsync(entity);
		await _context.SaveChangesAsync();
		return entity;
	}

	public async Task<TResult> AddAsync<TSource, TResult>(TSource entity)
	{
		var mappedEntity = _mapper.Map<T>(entity);
		await _context.AddAsync(mappedEntity);
		await _context.SaveChangesAsync();
		return _mapper.Map<TResult>(mappedEntity);
	}

	public async Task<T> GetAsync(int? id)
	{
		var result = await _context.Set<T>().FindAsync(id);
		if (result is null)
		{
			return null;
		}
		return result;
	}

	public async Task<TResult> GetAsync<TResult>(int? id)
	{
		var result = await _context.Set<T>().FindAsync(id);

		if (result is null)
		{
			throw new NotFoundException(typeof(T).Name, id.HasValue ? id : "No Key Provided");
		}

		return _mapper.Map<TResult>(result);
	}

	public async Task<List<T>> GetAllAsync()
	{
		return await _context.Set<T>().ToListAsync();
	}

	public async Task<List<TResult>> GetAllAsync<TResult>()
	{
		return await _context.Set<T>()
			.ProjectTo<TResult>(_mapper.ConfigurationProvider)
			.ToListAsync();
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

	public async Task UpdateAsync(T entity)
	{
		_context.Update(entity);
		await _context.SaveChangesAsync();
	}

	public async Task UpdateAsync<TSource>(int id, TSource entity)
	{
		var existingEntity = await GetAsync(id);

		var mappedEntity = _mapper.Map(entity, existingEntity);
		_context.Update(mappedEntity);
		await _context.SaveChangesAsync();
	}

	public async Task DeleteAsync(int id)
	{
		var entity = await GetAsync(id);

		_context.Set<T>().Remove(entity);
		await _context.SaveChangesAsync();
	}

	public async Task<bool> ExistsAsync(int id)
	{
		var entity = await GetAsync(id);
		return entity != null;
	}
}