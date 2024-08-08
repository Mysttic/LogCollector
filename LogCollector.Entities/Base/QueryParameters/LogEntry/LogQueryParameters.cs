public class LogQueryParameters : QueryParameters, ILogQueryParameters
{
	public string? DeviceId { get; set; }
	public string? ApplicationName { get; set; }
	public string? IpAddress { get; set; }
	public string? LogType { get; set; }
	public string? LogMessage { get; set; }
	public DateTimeOffset? StartDate { get; set; }
	public DateTimeOffset? EndDate { get; set; }

}