using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;

public class AlertRepository : GenericRepository<Alert>, IAlertRepository
{
	public AlertRepository(LogCollectorDbContext context, IMapper mapper, IDistributedCache cache) : base(context, mapper, cache)
	{
	}
}