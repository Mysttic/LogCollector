public interface ILogQueryParameters : IQueryParameters
{
	string? DeviceId { get; set; }
	string? ApplicationName { get; set; }
	string? IpAddress { get; set; }
	string? LogType { get; set; }
	string? LogMessage { get; set; }
	DateTimeOffset? StartDate { get; set; }
	DateTimeOffset? EndDate { get; set; }
}