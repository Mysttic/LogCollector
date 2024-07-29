public abstract class BaseLogEntryDto
{
	public string? DeviceId { get; set; }
	public string? ApplicationName { get; set; }
	public string? IpAddress { get; set; }
	public string? LogMessage { get; set; }
}
