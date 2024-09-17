public interface IAlertQueryParameters : IQueryParameters
{
	int MonitorId { get; set; }
	string? Message { get; set; }
	string? Content { get; set; }
}