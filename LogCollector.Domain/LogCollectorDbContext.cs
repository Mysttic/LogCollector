using Microsoft.EntityFrameworkCore;

public class LogCollectorDbContext : DbContext
{
	public LogCollectorDbContext(DbContextOptions<LogCollectorDbContext> options) : base(options)
	{
	}

	public DbSet<LogEntry> Logs { get; set; }
	public DbSet<Alert> Alerts { get; set; }
	public DbSet<Monitor> Monitors { get; set; }
}

