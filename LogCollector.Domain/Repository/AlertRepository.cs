using AutoMapper;

public class AlertRepository : GenericRepository<Alert>, IAlertRepository
{
	public AlertRepository(LogCollectorDbContext context, IMapper mapper) : base(context, mapper)
	{
	}
}