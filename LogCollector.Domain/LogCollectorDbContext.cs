using Microsoft.EntityFrameworkCore;

public class LogCollectorDbContext : DbContext
{
	public DbSet<LogEntry> Logs { get; set; }
	public DbSet<Alert> Alerts { get; set; }
	public DbSet<Monitor> Monitors { get; set; }

	public LogCollectorDbContext(DbContextOptions<LogCollectorDbContext> options) : base(options)
	{
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.Entity<Monitor>()
			.HasMany(m => m.Alerts)
			.WithOne(a => a.Monitor)
			.HasForeignKey(a => a.MonitorId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}

