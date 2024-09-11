public class MonitorQueryParameters : QueryParameters, IMonitorQueryParameters
{
	public string? IsActive { get; set; }
	public string? Name { get; set; }
	public string? Action { get; set; }
	public string? Query { get; set; }
}