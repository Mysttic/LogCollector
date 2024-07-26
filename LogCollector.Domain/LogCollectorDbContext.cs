using Microsoft.EntityFrameworkCore;

public class LogCollectorDbContext : DbContext
{
	public LogCollectorDbContext(DbContextOptions<LogCollectorDbContext> options) : base(options)
	{
	}

	public DbSet<LogEntry> Logs { get; set; }
}

