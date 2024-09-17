public class AlertQueryParameters : QueryParameters, IAlertQueryParameters
{
	public int MonitorId { get; set; }
	public string? Message { get; set; }
	public string? Content { get; set; }
}