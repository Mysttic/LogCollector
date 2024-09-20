public class AlertDto
{
	public string? Message { get; set; }
	public string? Content { get; set; }
	public int MonitorId { get; set; }
	public DateTime InvokedAt { get; set; }
}