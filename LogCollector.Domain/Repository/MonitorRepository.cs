using AutoMapper;

public class MonitorRepository : GenericRepository<Monitor>, IMonitorRepository
{
	public MonitorRepository(LogCollectorDbContext context, IMapper mapper) : base(context, mapper)
	{
	}
}