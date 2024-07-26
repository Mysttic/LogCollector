public class LogEntry
{
	public int Id { get; set; }
	public string? DeviceId { get; set; }
	public string? ApplicationName { get; set; }
	public string? IpAddress { get; set; }
	public string? LogMessage { get; set; }
	public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
